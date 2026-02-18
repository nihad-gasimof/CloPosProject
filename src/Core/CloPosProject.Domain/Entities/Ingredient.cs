    using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{

    public class Ingredient : BaseEntity
    {
        public string Name { get; private set; }
        public UnitType Unit { get; private set; }
        public IngredientCategory Category { get; private set; } 
        public decimal MinimumStock { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        
        public List<MenuItemIngredient> MenuItemIngredients { get; private set; } = new();
        public Inventory? Inventory { get; private set; }

        private Ingredient() { }

        public Ingredient(string name, UnitType unit, decimal minimumStock, IngredientCategory category)
        {
            Name = name;
            Unit = unit;
            MinimumStock = minimumStock;
            Category = category; 
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
        public void Update(string name, UnitType unit, decimal minimumStock, IngredientCategory category)
        {
            Name = name;
            Unit = unit;
            MinimumStock = minimumStock;
            Category = category;
        }
        public decimal CurrentStock => Inventory?.Quantity ?? 0;
        public decimal CurrentPrice => Inventory?.AverageUnitPrice ?? 0;
        public bool IsLowStock => CurrentStock < MinimumStock;

    
        public void Deactivate()
        {
            IsActive = false;
        }
    }
}