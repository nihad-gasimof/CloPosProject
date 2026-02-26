using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Category;
using MediatR;
using System;

namespace CloPosProject.Application.Features.Queries.Category
{
    public record GetCategoryByIdQuery(Guid Id) : IRequest<SimpleResponse<CategoryResponse>>;
}
