using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, SimpleResponse<string>>
    {
        private readonly IOrderService _service;
        public UpdateOrderStatusHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<string>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
            => await _service.UpdateOrderStatusAsync(request.OrderId, request.NewStatus);
    }
}
