using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class MarkAsPickedUpHandler : IRequestHandler<MarkAsPickedUpCommand, SimpleResponse<string>>
    {
        private readonly IOrderService _service;
        public MarkAsPickedUpHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(MarkAsPickedUpCommand request, CancellationToken cancellationToken)
            => await _service.MarkAsPickedUpAsync(request.OrderId);
    }
}
