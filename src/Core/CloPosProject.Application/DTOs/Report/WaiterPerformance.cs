namespace CloPosProject.Application.DTOs.Report
{
    public class WaiterPerformance
    {
        public string WaiterId { get; set; }
        public string WaiterName { get; set; }
        public int OrdersServed { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
