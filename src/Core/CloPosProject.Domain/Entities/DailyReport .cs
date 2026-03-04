using CloPosProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class DailyReport : BaseEntity
    {
        public DateTime ReportDate { get; private set; }
        public DateTime GeneratedAt { get; private set; }

        // Satış statistikaları
        public int TotalOrders { get; private set; }
        public int CompletedOrders { get; private set; }
        public int CancelledOrders { get; private set; }
        public decimal TotalRevenue { get; private set; }
        public decimal TotalTax { get; private set; }
        public decimal TotalDiscount { get; private set; }
        public decimal AverageOrderValue { get; private set; }

        // Sifariş növləri
        public int DineInOrders { get; private set; }
        public int TakeAwayOrders { get; private set; }
        public int DeliveryOrders { get; private set; }

        // Ödəniş
        public decimal CashPayments { get; private set; }
        public decimal CardPayments { get; private set; }

        // Məhsul statistikaları
        public string TopSellingItems { get; private set; } // JSON
        public string CategorySales { get; private set; } // JSON

        // Rezervasiya
        public int TotalReservations { get; private set; }
        public int CompletedReservations { get; private set; }
        public int NoShowReservations { get; private set; }
        public int CancelledReservations { get; private set; }

        // Ofisiant
        public string WaiterPerformance { get; private set; } // JSON

        // Inventory
        public string LowStockItems { get; private set; } // JSON
        public decimal InventoryValue { get; private set; }

        private DailyReport() { }

        public DailyReport(
            DateTime reportDate,
            int totalOrders,
            int completedOrders,
            int cancelledOrders,
            decimal totalRevenue,
            decimal totalTax,
            decimal totalDiscount,
            decimal averageOrderValue,
            int dineInOrders,
            int takeAwayOrders,
            int deliveryOrders,
            decimal cashPayments,
            decimal cardPayments,
            string topSellingItems,
            string categorySales,
            int totalReservations,
            int completedReservations,
            int noShowReservations,
            int cancelledReservations,
            string waiterPerformance,
            string lowStockItems,
            decimal inventoryValue)
        {
            ReportDate = reportDate.Date;
            GeneratedAt = DateTime.UtcNow;
            TotalOrders = totalOrders;
            CompletedOrders = completedOrders;
            CancelledOrders = cancelledOrders;
            TotalRevenue = totalRevenue;
            TotalTax = totalTax;
            TotalDiscount = totalDiscount;
            AverageOrderValue = averageOrderValue;
            DineInOrders = dineInOrders;
            TakeAwayOrders = takeAwayOrders;
            DeliveryOrders = deliveryOrders;
            CashPayments = cashPayments;
            CardPayments = cardPayments;
            TopSellingItems = topSellingItems;
            CategorySales = categorySales;
            TotalReservations = totalReservations;
            CompletedReservations = completedReservations;
            NoShowReservations = noShowReservations;
            CancelledReservations = cancelledReservations;
            WaiterPerformance = waiterPerformance;
            LowStockItems = lowStockItems;
            InventoryValue = inventoryValue;
        }
    }
    }
