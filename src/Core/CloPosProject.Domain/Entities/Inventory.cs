using CloPosProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public Guid IngredientId { get; private set; }
        public Ingredient Ingredient { get; private set; }

        public decimal Quantity { get; private set; }
        public decimal AverageUnitPrice { get; private set; }

        public void AddStock(decimal quantity, decimal unitPrice)
        {
            AverageUnitPrice = (AverageUnitPrice * Quantity + unitPrice * quantity) / (Quantity + quantity);
            Quantity += quantity;
        }

        public void RemoveStock(decimal quantity)
        {
            if (quantity > Quantity)
                throw new InvalidOperationException("Not enough stock");
            Quantity -= quantity;
        }
    }

}
