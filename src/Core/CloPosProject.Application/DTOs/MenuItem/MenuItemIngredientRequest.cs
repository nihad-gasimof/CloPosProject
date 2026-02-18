using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.MenuItem
{
    public record MenuItemIngredientRequest(
    Guid IngredientId,
    decimal Quantity
);
}
