using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.MenuItem;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.MenuItem
{
    public class SetAvailabilityHandler : IRequestHandler<SetAvailabilityCommand, SimpleResponse<string>>
    {
        private readonly IMenuItemService _service;
        public SetAvailabilityHandler(IMenuItemService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(SetAvailabilityCommand request, CancellationToken cancellationToken)
            => await _service.SetAvailabilityAsync(request.Id, request.IsAvailable);
    }
}
