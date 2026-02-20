using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Application.Features.Queries.Order;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, SimpleResponse<OrderResponse>>
    {
        private readonly IOrderService _service;
        public GetOrderByIdHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
            => await _service.GetByIdAsync(request.Id);
    }
}
