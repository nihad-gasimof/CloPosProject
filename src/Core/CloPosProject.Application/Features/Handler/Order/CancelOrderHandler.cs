using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, SimpleResponse<string>>
    {
        private readonly IOrderService _service;
        public CancelOrderHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
            => await _service.CancelOrderAsync(request.OrderId);
    }
}
