using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Ingredient
{
    public record AddStockCommand(Guid IngredientId, decimal Quantity, decimal UnitPrice) : IRequest<SimpleResponse<string>>;
}
