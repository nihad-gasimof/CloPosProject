using CloPosProject.Application.Abstract.Category;
using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using CloPosProject.Application.Features.Commands.Category;

namespace CloPosProject.Application.Features.Handler.Category
{
    public class DeactivateCategoryHandler : IRequestHandler<DeactivateCategoryCommand, SimpleResponse<string>>
    {
        private readonly ICategoryService _service;
        public DeactivateCategoryHandler(ICategoryService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(DeactivateCategoryCommand request, CancellationToken cancellationToken)
            => await _service.DeactivateCategoryAsync(request.Id);
    }
}
