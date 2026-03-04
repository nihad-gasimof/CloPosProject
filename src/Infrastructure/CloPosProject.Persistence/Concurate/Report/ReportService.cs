using CloPosProject.Application.Abstract.Report;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Report;
using CloPosProject.Domain.Entities;
using CloPosProject.Domain.Enums;
using CloPosProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.Concurate.Report
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SimpleResponse<string>> GenerateDailyReportAsync(DateTime date)
        {
            try
            {
                var reportDate = date.Date;


                // Əgər bu gün üçün hesabat varsa, skip et
                var existingReport = await _context.DailyReports
                    .FirstOrDefaultAsync(r => r.ReportDate == reportDate);

                if (existingReport != null)
                {
                    return new SimpleResponse<string>("Bu tarix üçün hesabat artıq mövcuddur");
                }

                var nextDay = reportDate.AddDays(1);

                // 1. ORDER STATİSTİKALARI
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.Payments)
                    .Where(o => o.OrderDate >= reportDate && o.OrderDate < nextDay)
                    .ToListAsync();

                var totalOrders = orders.Count;
                var completedOrders = orders.Count(o => o.Status == OrderStatus.Completed);
                var cancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled);

                var completedOrdersList = orders.Where(o => o.Status == OrderStatus.Completed).ToList();
                var totalRevenue = completedOrdersList.Sum(o => o.TotalAmount);
                var totalTax = completedOrdersList.Sum(o => o.Tax);
                var totalDiscount = completedOrdersList.Sum(o => o.Discount);
                var averageOrderValue = completedOrders > 0 ? totalRevenue / completedOrders : 0;

                // 2. SİFARİŞ NÖVLƏRİ
                var dineInOrders = orders.Count(o => o.OrderType == OrderType.DineIn);
                var takeAwayOrders = orders.Count(o => o.OrderType == OrderType.TakeAway);
                var deliveryOrders = orders.Count(o => o.OrderType == OrderType.Delivery);

                // 3. ÖDƏNLƏR (bu hissə payment sisteminizdən asılıdır)
                decimal cashPayments = 0;
                decimal cardPayments = 0;
                // TODO: Payment method-a görə hesabla

                // 4. ƏN ÇOX SATILAN MƏHSULLAR
                var topItems = await _context.Set<OrderItem>()
                    .Where(oi => oi.Order.OrderDate >= reportDate &&
                                 oi.Order.OrderDate < nextDay &&
                                 oi.Order.Status == OrderStatus.Completed)
                    .GroupBy(oi => new { oi.MenuItemId, oi.MenuItemName })
                    .Select(g => new TopSellingItem
                    {
                        MenuItemId = g.Key.MenuItemId,
                        MenuItemName = g.Key.MenuItemName,
                        QuantitySold = g.Sum(oi => oi.Quantity),
                        Revenue = g.Sum(oi => oi.Subtotal)
                    })
                    .OrderByDescending(x => x.QuantitySold)
                    .Take(10)
                    .ToListAsync();

                var topSellingItemsJson = JsonSerializer.Serialize(topItems);

                // 5. KATEQORİYA SATIŞ
                var categorySales = await _context.Set<OrderItem>()
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(m => m.Category)
                    .Where(oi => oi.Order.OrderDate >= reportDate &&
                                 oi.Order.OrderDate < nextDay &&
                                 oi.Order.Status == OrderStatus.Completed)
                    .GroupBy(oi => new { oi.MenuItem.CategoryId, oi.MenuItem.Category.Name })
                    .Select(g => new CategorySale
                    {
                        CategoryId = g.Key.CategoryId,
                        CategoryName = g.Key.Name,
                        ItemsSold = g.Sum(oi => oi.Quantity),
                        Revenue = g.Sum(oi => oi.Subtotal)
                    })
                    .ToListAsync();

                var categorySalesJson = JsonSerializer.Serialize(categorySales);

                // 6. REZERVASIYA STATİSTİKASI
                var reservations = await _context.Reservations
                    .Where(r => r.ReservationDate == reportDate)
                    .ToListAsync();

                var totalReservations = reservations.Count;
                var completedReservations = reservations.Count(r => r.Status == ReservationStatus.Completed);
                var noShowReservations = reservations.Count(r => r.Status == ReservationStatus.NoShow);
                var cancelledReservations = reservations.Count(r => r.Status == ReservationStatus.Cancelled);

                // 7. OFİSİANT PERFORMANSI
                var waiterPerformance = await _context.Orders
                    .Include(o => o.Waiter)
                    .Where(o => o.OrderDate >= reportDate &&
                                o.OrderDate < nextDay &&
                                o.WaiterId != null &&
                                o.Status == OrderStatus.Completed)
                    .GroupBy(o => new { o.WaiterId, o.Waiter.Name, o.Waiter.Surname })
                    .Select(g => new WaiterPerformance
                    {
                        WaiterId = g.Key.WaiterId ?? "",
                        WaiterName = $"{g.Key.Name} {g.Key.Surname}",
                        OrdersServed = g.Count(),
                        TotalRevenue = g.Sum(o => o.FinalAmount),
                        AverageOrderValue = g.Average(o => o.FinalAmount)
                    })
                    .OrderByDescending(w => w.TotalRevenue)
                    .ToListAsync();

                var waiterPerformanceJson = JsonSerializer.Serialize(waiterPerformance);

                // 8. STOK ANALİZİ
                var lowStockItems = await _context.Ingredients
    .Where(i => i.IsActive &&
                ((decimal?)i.Inventory.Quantity ?? 0m) < i.MinimumStock)
    .Select(i => new LowStockItem
    {
        IngredientId = i.Id,
        IngredientName = i.Name,
        CurrentStock = (decimal?)i.Inventory.Quantity ?? 0m,
        MinimumStock = i.MinimumStock,
        Unit = i.Unit.ToString(),
        Deficit = i.MinimumStock -
                  ((decimal?)i.Inventory.Quantity ?? 0m)
    })
    .ToListAsync();

                var lowStockItemsJson = JsonSerializer.Serialize(lowStockItems);

                // 9. INVENTORY DƏYƏR
                var inventoryValue = await _context.Inventories
                    .Include(i => i.Ingredient)
                    .Where(i => i.Ingredient.IsActive)
                    .SumAsync(i => i.Quantity * i.AverageUnitPrice);

                // 10. HESABAT YARAT
                var report = new DailyReport(
                    reportDate,
                    totalOrders,
                    completedOrders,
                    cancelledOrders,
                    totalRevenue,
                    totalTax,
                    totalDiscount,
                    averageOrderValue,
                    dineInOrders,
                    takeAwayOrders,
                    deliveryOrders,
                    cashPayments,
                    cardPayments,
                    topSellingItemsJson,
                    categorySalesJson,
                    totalReservations,
                    completedReservations,
                    noShowReservations,
                    cancelledReservations,
                    waiterPerformanceJson,
                    lowStockItemsJson,
                    inventoryValue
                );

                await _context.DailyReports.AddAsync(report);
                await _context.SaveChangesAsync();


                return new SimpleResponse<string>("Gündəlik hesabat uğurla yaradıldı");
            }
            catch (Exception ex)
            {
                return new SimpleResponse<string>($"Xəta: {ex.Message}");
            }
        }

        public async Task<SimpleResponse<DailyReportResponse>> GetDailyReportAsync(DateTime date)
        {
            try
            {
                var reportDate = date.Date;

                var report = await _context.DailyReports
                    .FirstOrDefaultAsync(r => r.ReportDate == reportDate);

                if (report == null)
                    return new SimpleResponse<DailyReportResponse>("Bu tarix üçün hesabat tapılmadı");

                var response = new DailyReportResponse
                {
                    ReportDate = report.ReportDate,
                    GeneratedAt = report.GeneratedAt,
                    TotalOrders = report.TotalOrders,
                    CompletedOrders = report.CompletedOrders,
                    CancelledOrders = report.CancelledOrders,
                    TotalRevenue = report.TotalRevenue,
                    TotalTax = report.TotalTax,
                    TotalDiscount = report.TotalDiscount,
                    AverageOrderValue = report.AverageOrderValue,
                    DineInOrders = report.DineInOrders,
                    TakeAwayOrders = report.TakeAwayOrders,
                    DeliveryOrders = report.DeliveryOrders,
                    CashPayments = report.CashPayments,
                    CardPayments = report.CardPayments,
                    TopSellingItems = JsonSerializer.Deserialize<List<TopSellingItem>>(report.TopSellingItems),
                    CategorySales = JsonSerializer.Deserialize<List<CategorySale>>(report.CategorySales),
                    TotalReservations = report.TotalReservations,
                    CompletedReservations = report.CompletedReservations,
                    NoShowReservations = report.NoShowReservations,
                    CancelledReservations = report.CancelledReservations,
                    WaiterPerformance = JsonSerializer.Deserialize<List<WaiterPerformance>>(report.WaiterPerformance),
                    LowStockItems = JsonSerializer.Deserialize<List<LowStockItem>>(report.LowStockItems),
                    InventoryValue = report.InventoryValue
                };

                return new SimpleResponse<DailyReportResponse>(response);
            }
            catch (Exception ex)
            {
                return new SimpleResponse<DailyReportResponse>($"Xəta: {ex.Message}");
            }
        }

        public async Task<SimpleResponse<List<DailyReportSummary>>> GetReportRangeAsync(
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                var reports = await _context.DailyReports
                    .Where(r => r.ReportDate >= startDate.Date && r.ReportDate <= endDate.Date)
                    .OrderByDescending(r => r.ReportDate)
                    .ToListAsync();

                var summaries = reports.Select(r => new DailyReportSummary
                {
                    ReportDate = r.ReportDate,
                    TotalOrders = r.TotalOrders,
                    CompletedOrders = r.CompletedOrders,
                    TotalRevenue = r.TotalRevenue,
                    AverageOrderValue = r.AverageOrderValue
                }).ToList();

                return new SimpleResponse<List<DailyReportSummary>>(summaries);
            }
            catch (Exception ex)
            {
                return new SimpleResponse<List<DailyReportSummary>>($"Xəta: {ex.Message}");
            }
        }

        public async Task<SimpleResponse<MonthlyReportResponse>> GetMonthlyReportAsync(int year, int month)
        {
            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var reports = await _context.DailyReports
                    .Where(r => r.ReportDate >= startDate && r.ReportDate <= endDate)
                    .ToListAsync();

                if (!reports.Any())
                    return new SimpleResponse<MonthlyReportResponse>("Bu ay üçün hesabat tapılmadı");

                var monthlyReport = new MonthlyReportResponse
                {
                    Year = year,
                    Month = month,
                    TotalOrders = reports.Sum(r => r.TotalOrders),
                    CompletedOrders = reports.Sum(r => r.CompletedOrders),
                    CancelledOrders = reports.Sum(r => r.CancelledOrders),
                    TotalRevenue = reports.Sum(r => r.TotalRevenue),
                    TotalTax = reports.Sum(r => r.TotalTax),
                    TotalDiscount = reports.Sum(r => r.TotalDiscount),
                    AverageOrderValue = reports.Average(r => r.AverageOrderValue),
                    TotalReservations = reports.Sum(r => r.TotalReservations),
                    DailyReports = reports.Select(r => new DailyReportSummary
                    {
                        ReportDate = r.ReportDate,
                        TotalOrders = r.TotalOrders,
                        CompletedOrders = r.CompletedOrders,
                        TotalRevenue = r.TotalRevenue,
                        AverageOrderValue = r.AverageOrderValue
                    }).OrderBy(r => r.ReportDate).ToList()
                };

                return new SimpleResponse<MonthlyReportResponse>(monthlyReport);
            }
            catch (Exception ex)
            {
                return new SimpleResponse<MonthlyReportResponse>($"Xəta: {ex.Message}");
            }
        }
    }
}
