using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Ingredient
{
    public record UpdateIngredientCommand(Guid Id, UpdateIngredientDto Dto) : IRequest<SimpleResponse<string>>;
}
