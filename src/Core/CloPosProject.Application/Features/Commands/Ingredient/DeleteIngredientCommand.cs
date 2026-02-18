using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Ingredient
{
    public record DeleteIngredientCommand(Guid Id) : IRequest<SimpleResponse<string>>;
}
