using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Category;
using MediatR;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Queries.Category
{
    public record GetAllCategoriesQuery(bool? IsActive) : IRequest<SimpleResponse<List<CategoryResponse>>>;
}
