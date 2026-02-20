using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.MenuItem;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.MenuItem
{
    public class DeleteMenuItemHandler : IRequestHandler<DeleteMenuItemCommand, SimpleResponse<string>>
    {
        private readonly IMenuItemService _service;
        public DeleteMenuItemHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
            => await _service.DeleteMenuItemAsync(request.Id);
    }
}
