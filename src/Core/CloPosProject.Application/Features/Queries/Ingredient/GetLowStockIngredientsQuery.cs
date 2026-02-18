using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using MediatR;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Queries.Ingredient
{
    public record GetLowStockIngredientsQuery() : IRequest<SimpleResponse<List<LowStockResponseDto>>>;
}
