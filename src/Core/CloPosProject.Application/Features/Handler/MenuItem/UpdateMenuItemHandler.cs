using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.MenuItem;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.MenuItem
{
    public class UpdateMenuItemHandler : IRequestHandler<UpdateMenuItemCommand, SimpleResponse<string>>
    {
        private readonly IMenuItemService _service;
        public UpdateMenuItemHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
            => await _service.UpdateMenuItemAsync(request.Id, request.Dto.Name, request.Dto.Description, request.Dto.Price, request.Dto.PreparationTime, request.Dto.CategoryId, null);
    }
}
