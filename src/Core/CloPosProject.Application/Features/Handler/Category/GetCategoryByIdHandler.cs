using CloPosProject.Application.Abstract.Category;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Category;
using CloPosProject.Application.Features.Queries.Category;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace CloPosProject.Application.Features.Handler.Category
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, SimpleResponse<CategoryResponse>>
    {
        private readonly ICategoryService _service;
        public GetCategoryByIdHandler(ICategoryService service) => _service = service;
        public async Task<SimpleResponse<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
            => await _service.GetByIdAsync(request.Id);
    }
}
