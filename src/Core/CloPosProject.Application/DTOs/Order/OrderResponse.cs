using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Order
{
    public record OrderResponse
    {
        // Basic order info
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public OrderStatus Status { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime CreatedAt { get; set; }

        // Table info (null if TakeAway/Delivery)
        public Guid? TableId { get; set; }
        public Guid? WaiterId { get; set; }
        public string? TableName { get; set; }

        // Customer / Delivery info
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? DeliveryProvider { get; set; }
        public string? DeliveryInstructions { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }

        // Pickup info
        public DateTime? PickupTime { get; set; }
        public bool IsPickedUp { get; set; }

        // Order financials
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal FinalAmount { get; set; }

        // Extra
        public string? Notes { get; set; }

        // Items
        public List<OrderItemResponse> Items { get; set; } = new();
    }
}
