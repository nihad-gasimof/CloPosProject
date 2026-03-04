namespace CloPosProject.Application.DTOs.Report
{
    public class DailyReportSummary
    {
        public DateTime ReportDate { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
