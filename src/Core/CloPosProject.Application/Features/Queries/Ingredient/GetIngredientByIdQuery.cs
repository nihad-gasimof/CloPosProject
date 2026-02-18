using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Queries.Ingredient
{
    public record GetIngredientByIdQuery(Guid Id) : IRequest<SimpleResponse<IngredientResponseDto>>;
}
