using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.MenuItem;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.MenuItem
{
    public class DeductIngredientsHandler : IRequestHandler<DeductIngredientsCommand, SimpleResponse<string>>
    {
        private readonly IMenuItemService _service;
        public DeductIngredientsHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(DeductIngredientsCommand request, CancellationToken cancellationToken)
            => await _service.DeductIngredientsForOrderAsync(request.MenuItemId, request.Quantity);
    }
}
