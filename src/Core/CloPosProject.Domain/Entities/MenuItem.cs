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
        public List<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();
    }
}
