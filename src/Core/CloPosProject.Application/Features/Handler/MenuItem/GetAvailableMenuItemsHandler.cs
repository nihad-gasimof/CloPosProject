using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using CloPosProject.Application.Features.Queries.MenuItem;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.MenuItem
{
    public class GetAvailableMenuItemsHandler : IRequestHandler<GetAvailableMenuItemsQuery, SimpleResponse<List<MenuItemSummaryResponse>>>
    {
        private readonly IMenuItemService _service;
        public GetAvailableMenuItemsHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<List<MenuItemSummaryResponse>>> Handle(GetAvailableMenuItemsQuery request, CancellationToken cancellationToken)
            => await _service.GetAvailableMenuItemsAsync();
    }
}
