using CloPosProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class MenuItemIngredient:BaseEntity
    {
        public Guid MenuItemId { get; private set; }
        public MenuItem MenuItem { get; private set; }

        public Guid IngredientId { get; private set; }
        public Ingredient Ingredient { get; private set; }

        public decimal Quantity { get; private set; } 
        private MenuItemIngredient() { }

        public MenuItemIngredient(Guid menuItemId, Guid ingredientId, decimal quantity)
        {
            Id = Guid.NewGuid();
            MenuItemId = menuItemId;
            IngredientId = ingredientId;
            Quantity = quantity;
        }

        public void UpdateQuantity(decimal newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Miqdar müsbət olmalıdır");
            Quantity = newQuantity;
        }
    }
}
