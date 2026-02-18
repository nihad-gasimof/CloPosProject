using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Ingredient
{
    public record LowStockResponseDto(
        Guid Id,
        string Name,
         decimal MinimumStock,
       decimal CurrentStock,
       string UnitType,
        decimal LowStock

        );

}
