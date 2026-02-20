using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Application.Features.Queries.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class GetOrderByOrderNumberHandler : IRequestHandler<GetOrderByOrderNumberQuery, SimpleResponse<OrderResponse>>
    {
        private readonly IOrderService _service;
        public GetOrderByOrderNumberHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<OrderResponse>> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
            => await _service.GetByOrderNumberAsync(request.OrderNumber);
    }
}
