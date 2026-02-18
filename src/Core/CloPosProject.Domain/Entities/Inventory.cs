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

        private Inventory() { }
        public Inventory(Guid ingredientId, decimal quantity, decimal unitPrice)
        {
            Id = Guid.NewGuid();
            IngredientId = ingredientId;
            Quantity = quantity;
            AverageUnitPrice = unitPrice;
        }
    public void AddStock(decimal quantity, decimal unitPrice)
{
    if (quantity <= 0)
        throw new ArgumentException("Quantity must be greater than 0");

    var totalValue = AverageUnitPrice * Quantity;
    var newValue = unitPrice * quantity;

    Quantity += quantity;
    AverageUnitPrice = Quantity == 0 ? 0 : (totalValue + newValue) / Quantity;
}

        public void RemoveStock(decimal quantity)
        {
            if (quantity > Quantity)
                throw new InvalidOperationException("Not enough stock");
            Quantity -= quantity;
        }
    }

}
