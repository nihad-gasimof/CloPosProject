using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Application.Features.Commands.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class CreateDeliveryOrderHandler : IRequestHandler<CreateDeliveryOrderCommand, SimpleResponse<System.Guid>>
    {
        private readonly IOrderService _service;
        public CreateDeliveryOrderHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<System.Guid>> Handle(CreateDeliveryOrderCommand request, CancellationToken cancellationToken)
            => await _service.CreateDeliveryOrderAsync(request.Request.CustomerName, request.Request.CustomerPhone, request.Request.DeliveryAddress, request.Request.DeliveryProvider, request.Request.DeliveryFee, request.Request.DeliveryInstructions, request.Request.Notes, request.Request.Items);
    }
}
