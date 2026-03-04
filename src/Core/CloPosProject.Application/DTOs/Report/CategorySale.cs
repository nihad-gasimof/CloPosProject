namespace CloPosProject.Application.DTOs.Report
{
    public class CategorySale
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ItemsSold { get; set; }
        public decimal Revenue { get; set; }
    }
}
