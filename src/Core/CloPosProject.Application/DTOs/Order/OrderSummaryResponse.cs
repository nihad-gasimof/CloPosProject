using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Order
{
    public record OrderSummaryResponse
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public OrderStatus Status { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? TableName { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }

        public int TotalItemCount { get; set; }
        public decimal FinalAmount { get; set; }
    }
}
