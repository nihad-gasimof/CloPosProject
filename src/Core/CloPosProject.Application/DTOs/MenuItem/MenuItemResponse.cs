using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.MenuItem
{
    public record MenuItemResponse(
     Guid Id,
     string Name,
     string Description,
     decimal Price,
     string ImageUrl,
     bool IsAvailable,
     int PreparationTime,
     Guid CategoryId,
     string CategoryName,
     bool CanBePrepared, 
     List<MenuItemIngredientResponse> Ingredients
 );
}
