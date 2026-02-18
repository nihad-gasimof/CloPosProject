using CloPosProject.Domain.Entities;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Ingredient
{
    public record IngredientResponseDto(
       Guid Id,
       string Name,
       UnitType Unit,
       IngredientCategory Category,
       decimal MinimumStock,
       decimal CurrentStock,
       decimal CurrentPrice,
       bool IsLowStock,
       bool IsActive,
       DateTime CreatedAt
   );
}
