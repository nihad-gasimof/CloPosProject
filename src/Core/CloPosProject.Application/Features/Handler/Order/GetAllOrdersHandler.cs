using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Application.Features.Queries.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Handler.Order
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, SimpleResponse<List<OrderSummaryResponse>>>
    {
        private readonly IOrderService _service;
        public GetAllOrdersHandler(IOrderService service) => _service = service;
        public async Task<SimpleResponse<List<OrderSummaryResponse>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
            => await _service.GetAllAsync(request.PageNumber, request.PageSize, request.Status, request.OrderType, request.Date);
    }
}
