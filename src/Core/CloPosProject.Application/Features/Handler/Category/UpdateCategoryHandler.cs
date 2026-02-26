using CloPosProject.Application.Abstract.Category;
using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using CloPosProject.Application.Features.Commands.Category;

namespace CloPosProject.Application.Features.Handler.Category
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, SimpleResponse<string>>
    {
        private readonly ICategoryService _service;
        public UpdateCategoryHandler(ICategoryService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            => await _service.UpdateCategoryAsync(request.Id, request.Name, request.Description, request.DisplayOrder);
    }
}
