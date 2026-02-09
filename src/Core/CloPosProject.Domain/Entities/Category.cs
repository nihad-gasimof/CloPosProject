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
        public string Name { get; set; }= string.Empty;
        public string Description { get; set; }= string.Empty;
        public bool IsActive { get; set; } = true;
        public List<MenuItem> MenuItems { get; private set; } = new List<MenuItem>();
    }
}
