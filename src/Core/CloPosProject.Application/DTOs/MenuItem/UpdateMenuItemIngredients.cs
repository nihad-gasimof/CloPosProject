using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.MenuItem
{
    public record UpdateMenuItemIngredients(
    List<MenuItemIngredientRequest> Ingredients
);
}
