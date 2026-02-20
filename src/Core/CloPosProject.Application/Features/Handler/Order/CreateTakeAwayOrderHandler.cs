using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Application.Features.Commands.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class CreateTakeAwayOrderHandler : IRequestHandler<CreateTakeAwayOrderCommand, SimpleResponse<System.Guid>>
    {
        private readonly IOrderService _service;
        public CreateTakeAwayOrderHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<System.Guid>> Handle(CreateTakeAwayOrderCommand request, CancellationToken cancellationToken)
            => await _service.CreateTakeAwayOrderAsync(request.Request.CustomerName, request.Request.CustomerPhone, request.Request.PickupTime, request.Request.Notes, request.Request.Items);
    }
}
