using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Commands.Ingredient
{
    public record CreateIngredientCommand(CreateIngredientDto Dto) : IRequest<SimpleResponse<Guid>>;
}
