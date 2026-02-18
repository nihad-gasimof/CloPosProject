using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.MenuItem
{
    public record UpdateMenuItem(
    string Name,
    string Description,
    decimal Price,
    int PreparationTime,
    Guid CategoryId,
    string ImageUrl
);
}
