using CloPosProject.Application.Abstract.Category;
using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using CloPosProject.Application.Features.Commands.Category;

namespace CloPosProject.Application.Features.Handler.Category
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, SimpleResponse<Guid>>
    {
        private readonly ICategoryService _service;
        public CreateCategoryHandler(ICategoryService service) => _service = service;
        public async Task<SimpleResponse<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            => await _service.CreateCategoryAsync(request.Name, request.Description, request.DisplayOrder);
    }
}
