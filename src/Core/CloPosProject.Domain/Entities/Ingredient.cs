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
        public string NameAz { get; private set; }
        public UnitType Unit { get; private set; }

        public decimal CurrentStock { get; private set; }
        public decimal MinimumStock { get; private set; }
        public decimal UnitPrice { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public List<MenuItem> MenuItems { get; private set; } = new List<MenuItem>();
    }
}