using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Report
{
    public class DailyReportResponse
    {
        public DateTime ReportDate { get; set; }
        public DateTime GeneratedAt { get; set; }

        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal AverageOrderValue { get; set; }

        public int DineInOrders { get; set; }
        public int TakeAwayOrders { get; set; }
        public int DeliveryOrders { get; set; }

        public decimal CashPayments { get; set; }
        public decimal CardPayments { get; set; }

        public List<TopSellingItem> TopSellingItems { get; set; }
        public List<CategorySale> CategorySales { get; set; }

        public int TotalReservations { get; set; }
        public int CompletedReservations { get; set; }
        public int NoShowReservations { get; set; }
        public int CancelledReservations { get; set; }

        public List<WaiterPerformance> WaiterPerformance { get; set; }

        public List<LowStockItem> LowStockItems { get; set; }
        public decimal InventoryValue { get; set; }
    }
}
