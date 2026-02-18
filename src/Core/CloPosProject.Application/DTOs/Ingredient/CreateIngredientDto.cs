using CloPosProject.Domain.Entities;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Ingredient
{
    public record CreateIngredientDto
    {
        public string Name { get; set; }
        public UnitType Unit { get; set; }
        public decimal MinimumStock { get; set; }
        public IngredientCategory Category { get; set; }
        public decimal InitialQuantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
