using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using CloPosProject.Application.Features.Queries.MenuItem;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.MenuItem
{
    public class GetMenuItemByIdHandler : IRequestHandler<GetMenuItemByIdQuery, SimpleResponse<MenuItemResponse>>
    {
        private readonly IMenuItemService _service;
        public GetMenuItemByIdHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<MenuItemResponse>> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
            => await _service.GetByIdAsync(request.Id);
    }
}
