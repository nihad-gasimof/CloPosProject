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
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("dine-in")]
        [ProducesResponseType(typeof(SimpleResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<Guid>>> CreateDineIn([FromBody] CreateDineInOrderRequest request)
        {
            var result = await _mediator.Send(new CreateDineInOrderCommand(request));
            return Ok(result);
        }

        [HttpPost("delivery")]
        [ProducesResponseType(typeof(SimpleResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<Guid>>> CreateDelivery([FromBody] CreateDeliveryOrderRequest request)
        {
            var result = await _mediator.Send(new CreateDeliveryOrderCommand(request));
            return Ok(result);
        }

        [HttpPost("takeaway")]
        [ProducesResponseType(typeof(SimpleResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<Guid>>> CreateTakeAway([FromBody] CreateTakeAwayOrderRequest request)
        {
            var result = await _mediator.Send(new CreateTakeAwayOrderCommand(request));
            return Ok(result);
        }

        [HttpPost("{id:guid}/cancel")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> Cancel([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new CancelOrderCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/apply-discount")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<string>>> ApplyDiscount([FromRoute] Guid id, [FromQuery] decimal discount)
        {
            var result = await _mediator.Send(new ApplyDiscountCommand(id, discount));
            return Ok(result);
        }

        [HttpPost("{id:guid}/mark-pickedup")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> MarkAsPickedUp([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new MarkAsPickedUpCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/status")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> UpdateStatus([FromRoute] Guid id, [FromQuery] Domain.Enums.OrderStatus newStatus)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand(id, newStatus));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<OrderResponse>>> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("bynumber")]
        [ProducesResponseType(typeof(SimpleResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<OrderResponse>>> GetByOrderNumber([FromQuery] string orderNumber)
        {
            var result = await _mediator.Send(new GetOrderByOrderNumberQuery(orderNumber));
            return Ok(result);
        }

        [HttpGet("active")]
        [ProducesResponseType(typeof(SimpleResponse<List<OrderSummaryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<OrderSummaryResponse>>>> GetActive([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetActiveOrdersQuery(pageNumber, pageSize));
            return Ok(result);
        }

        [HttpGet("active-delivery")]
        [ProducesResponseType(typeof(SimpleResponse<List<OrderSummaryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<OrderSummaryResponse>>>> GetActiveDelivery([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetActiveDeliveryOrdersQuery(pageNumber, pageSize));
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SimpleResponse<List<OrderSummaryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<OrderSummaryResponse>>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, [FromQuery] Domain.Enums.OrderStatus? status = null, [FromQuery] Domain.Enums.OrderType? orderType = null, [FromQuery] DateTime? date = null)
        {
            var result = await _mediator.Send(new GetAllOrdersQuery(pageNumber, pageSize, status, orderType, date));
            return Ok(result);
        }

        [HttpGet("pending-takeaway")]
        [ProducesResponseType(typeof(SimpleResponse<List<OrderSummaryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<OrderSummaryResponse>>>> GetPendingTakeAway([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetPendingTakeAwayOrdersQuery(pageNumber, pageSize));
            return Ok(result);
        }

        [HttpGet("table/{tableId:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<List<OrderSummaryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<OrderSummaryResponse>>>> GetTableOrders([FromRoute] Guid tableId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetTableOrdersQuery(tableId, pageNumber, pageSize));
            return Ok(result);
        }
    }
}
