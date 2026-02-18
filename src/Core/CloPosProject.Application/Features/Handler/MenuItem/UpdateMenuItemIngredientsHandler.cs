using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.MenuItem;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.MenuItem
{
    public class UpdateMenuItemIngredientsHandler : IRequestHandler<UpdateMenuItemIngredientsCommand, SimpleResponse<string>>
    {
        private readonly IMenuItemService _service;
        public UpdateMenuItemIngredientsHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(UpdateMenuItemIngredientsCommand request, CancellationToken cancellationToken)
            => await _service.UpdateMenuItemIngredientsAsync(request.MenuItemId, request.Ingredients);
    }
}
