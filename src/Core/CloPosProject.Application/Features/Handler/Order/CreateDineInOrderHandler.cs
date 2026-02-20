using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Application.Features.Commands.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class CreateDineInOrderHandler : IRequestHandler<CreateDineInOrderCommand, SimpleResponse<System.Guid>>
    {
        private readonly IOrderService _service;
        public CreateDineInOrderHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<System.Guid>> Handle(CreateDineInOrderCommand request, CancellationToken cancellationToken)
            => await _service.CreateDineInOrderAsync(request.Request.TableId, System.Guid.Parse(request.Request.WaiterId), request.Request.TableNumber, request.Request.Notes, request.Request.Items);
    }
}
