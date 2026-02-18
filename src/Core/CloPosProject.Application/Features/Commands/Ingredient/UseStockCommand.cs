using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Ingredient
{
    public record UseStockCommand(Guid IngredientId, decimal Quantity) : IRequest<SimpleResponse<string>>;
}
