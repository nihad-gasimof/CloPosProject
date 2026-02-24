using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.Order;
using CloPosProject.Application.DTOs.Payment;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CloPosProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentController(IMediator mediator) => _mediator = mediator;

        [HttpPost("create/{orderId:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<PurchaseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePaymentForOrder([FromRoute] Guid orderId, [FromQuery] string redirectUrl)
        {
            var result = await _mediator.Send(new CreatePaymentForOrderCommand(orderId, redirectUrl));
            string url = $"{result.Data.Order.HppUrl}?password={result.Data.Order.Password}&id={result.Data.Order.Id}";
            return Ok(url);
        }

        [HttpPost("verify")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyAndComplete([FromQuery] int purchaseId)
        {
            var result = await _mediator.Send(new VerifyAndCompletePaymentCommand(purchaseId));
            return Ok(result);
        }
    }
}
