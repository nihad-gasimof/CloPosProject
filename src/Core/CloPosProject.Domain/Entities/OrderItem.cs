using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class OrderItem: BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }

        public Guid MenuItemId { get; private set; }
        public MenuItem MenuItem { get; private set; }

        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Total => UnitPrice * Quantity;

        public OrderType Status { get; private set; }
        public string SpecialInstructions { get; private set; }
    }
}
