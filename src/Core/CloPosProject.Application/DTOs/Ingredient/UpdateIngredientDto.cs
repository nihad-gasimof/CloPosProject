using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Ingredient
{
    public record UpdateIngredientDto
    {
        public string Name { get; set; }

        public UnitType Unit { get; set; }

        public decimal MinimumStock { get; set; }

        public IngredientCategory Category { get; set; }
    }
}
