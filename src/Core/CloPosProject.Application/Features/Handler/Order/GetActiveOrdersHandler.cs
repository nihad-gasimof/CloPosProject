using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Application.Features.Queries.Order;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class GetActiveOrdersHandler : IRequestHandler<GetActiveOrdersQuery, SimpleResponse<List<OrderSummaryResponse>>>
    {
        private readonly IOrderService _service;
        public GetActiveOrdersHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<List<OrderSummaryResponse>>> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
            => await _service.GetActiveOrdersAsync(request.PageNumber, request.PageSize);
    }
}
