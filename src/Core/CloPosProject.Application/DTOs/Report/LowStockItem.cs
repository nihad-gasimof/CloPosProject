namespace CloPosProject.Application.DTOs.Report
{
    public class LowStockItem
    {
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public string Unit { get; set; }
        public decimal Deficit { get; set; }
    }
}
