using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.MenuItem
{
    public record MenuItemSummaryResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    bool IsAvailable,
    int PreparationTime,
    string CategoryName,
    bool CanBePrepared
);
}
