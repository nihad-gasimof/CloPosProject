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
    public class GetTableOrdersHandler : IRequestHandler<GetTableOrdersQuery, SimpleResponse<List<OrderSummaryResponse>>>
    {
        private readonly IOrderService _service;
        public GetTableOrdersHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<List<OrderSummaryResponse>>> Handle(GetTableOrdersQuery request, CancellationToken cancellationToken)
            => await _service.GetTableOrdersAsync(request.TableId, request.PageNumber, request.PageSize);
    }
}
