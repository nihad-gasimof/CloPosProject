namespace CloPosProject.Application.DTOs.Payment
{
    public class OrderCreateDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
    };

    }
