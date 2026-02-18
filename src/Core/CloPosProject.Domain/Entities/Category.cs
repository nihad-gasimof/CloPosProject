using CloPosProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsActive { get; private set; }

        public List<MenuItem> MenuItems { get; private set; } = new();

        private Category() { }

        public Category(string name, string description = null, int displayOrder = 0)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            DisplayOrder = displayOrder;
            IsActive = true;
        }

        public void UpdateDetails(string name, string description, int displayOrder)
        {
            Name = name;
            Description = description;
            DisplayOrder = displayOrder;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}
