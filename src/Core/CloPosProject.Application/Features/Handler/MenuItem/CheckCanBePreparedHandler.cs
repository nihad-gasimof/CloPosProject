using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Queries.MenuItem;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.MenuItem
{
    public class CheckCanBePreparedHandler : IRequestHandler<CheckCanBePreparedQuery, SimpleResponse<bool>>
    {
        private readonly IMenuItemService _service;
        public CheckCanBePreparedHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<bool>> Handle(CheckCanBePreparedQuery request, CancellationToken cancellationToken)
            => await _service.CheckIfCanBePreparedAsync(request.MenuItemId);
    }
}
