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
    public class GetAllMenuItemsHandler : IRequestHandler<GetAllMenuItemsQuery, SimpleResponse<List<MenuItemSummaryResponse>>>
    {
        private readonly IMenuItemService _service;
        public GetAllMenuItemsHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<List<MenuItemSummaryResponse>>> Handle(GetAllMenuItemsQuery request, CancellationToken cancellationToken)
            => await _service.GetAllAsync(request.IsAvailable, request.CategoryId);
    }
}
