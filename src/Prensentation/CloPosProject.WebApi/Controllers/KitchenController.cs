using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Order;
using CloPosProject.Application.Features.Commands.Order;
using CloPosProject.Application.Features.Queries.Order;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloPosProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KitchenController : ControllerBase
    {
        private readonly IMediator _mediator;
        public KitchenController(IMediator mediator) => _mediator = mediator;

        [HttpGet("orders")]
        public async Task<ActionResult<SimpleResponse<List<OrderSummaryResponse>>>> GetActiveOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetActiveOrdersQuery(pageNumber, pageSize));
            return Ok(result);
        }

        [HttpPost("orders/{orderId:guid}/status")]
        public async Task<ActionResult<SimpleResponse<string>>> ChangeOrderStatus([FromRoute] Guid orderId, [FromQuery] CloPosProject.Domain.Enums.OrderStatus newStatus)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand(orderId, newStatus));
            return Ok(result);
        }
    }
}
