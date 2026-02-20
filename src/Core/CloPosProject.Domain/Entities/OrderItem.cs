using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }

        public Guid MenuItemId { get; private set; }
        public MenuItem MenuItem { get; private set; }

        public string MenuItemName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal Subtotal { get; private set; }
        public string SpecialInstructions { get; private set; }

        private OrderItem() { }

        public OrderItem(
            Guid orderId,
            Guid menuItemId,
            string menuItemName,
            decimal unitPrice,
            int quantity,
            string specialInstructions = null)
        {
            OrderId = orderId;
            MenuItemId = menuItemId;
            MenuItemName = menuItemName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            SpecialInstructions = specialInstructions;
            Subtotal = unitPrice * quantity;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Miqdar müsbət olmalıdır");

            Quantity = newQuantity;
            Subtotal = UnitPrice * Quantity;
        }
    }
}
