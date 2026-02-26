using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.MenuItem
{
    public record CreateMenuItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PreparationTime { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public List<MenuItemIngredientRequest> Ingredients { get; set; } = new();
    }
}
