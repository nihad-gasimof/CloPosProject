using CloPosProject.Application.Abstract.Category;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Category;
using CloPosProject.Application.Features.Queries.Category;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CloPosProject.Application.Features.Handler.Category
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, SimpleResponse<List<CategoryResponse>>>
    {
        private readonly ICategoryService _service;
        public GetAllCategoriesHandler(ICategoryService service) => _service = service;
        public async Task<SimpleResponse<List<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
            => await _service.GetAllAsync(request.IsActive);
    }
}
