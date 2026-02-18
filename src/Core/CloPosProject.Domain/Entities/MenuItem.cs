using CloPosProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class MenuItem: BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string ImageUrl { get; private set; }
        public bool IsAvailable { get; private set; }
        public int PreparationTime { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }
        public List<MenuItemIngredient> MenuItemIngredients { get; private set; } = new();
        private MenuItem() { }
        public MenuItem(
         string name,
         string description,
         decimal price,
         int preparationTime,
         Guid categoryId,
         string imageUrl = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            PreparationTime = preparationTime;
            CategoryId = categoryId;
            ImageUrl = imageUrl;
            IsAvailable = true;
        }
        public void UpdateDetails(string name, string description, decimal price, int preparationTime, string imageUrl)
        {
            Name = name;
            Description = description;
            Price = price;
            PreparationTime = preparationTime;
            if (!string.IsNullOrEmpty(imageUrl))
                ImageUrl = imageUrl;
        }
        public void UpdateCategory(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public void SetAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;
        }

        public void MakeAvailable() => IsAvailable = true;
        public void MakeUnavailable() => IsAvailable = false;
    
}
}
