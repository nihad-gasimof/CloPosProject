namespace CloPosProject.Application.DTOs.Report
{
    public class TopSellingItem
    {
        public Guid MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }
}
