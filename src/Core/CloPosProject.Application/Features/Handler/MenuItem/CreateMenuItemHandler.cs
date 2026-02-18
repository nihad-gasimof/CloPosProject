using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using CloPosProject.Application.Features.Commands.MenuItem;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.MenuItem
{
    public class CreateMenuItemHandler : IRequestHandler<CreateMenuItemCommand, SimpleResponse<System.Guid>>
    {
        private readonly IMenuItemService _service;
        public CreateMenuItemHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<System.Guid>> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
            => await _service.CreateMenuItemAsync(request.Dto.Name, request.Dto.Description, request.Dto.Price, request.Dto.PreparationTime, request.Dto.CategoryId, request.Dto.ImageUrl, request.Dto.Ingredients);
    }
}
