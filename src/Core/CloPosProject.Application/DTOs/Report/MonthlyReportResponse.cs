namespace CloPosProject.Application.DTOs.Report
{
    public class MonthlyReportResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalReservations { get; set; }
        public List<DailyReportSummary> DailyReports { get; set; }
    }
}
