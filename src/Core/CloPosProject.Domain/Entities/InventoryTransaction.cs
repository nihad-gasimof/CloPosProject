using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class InventoryTransaction : BaseEntity
    {
        public Guid IngredientId { get; private set; }
        public Ingredient Ingredient { get; private set; }

        public InventoryTransactionType Type { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public int UserId { get; private set; }
        public User User { get; private set; }

        public string SupplierName { get; private set; }
        public string InvoiceNumber { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public DateTime CreatedAt { get; private set; }

    }
}
