using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Reservation;
using CloPosProject.Application.Features.Commands.Reservation;
using CloPosProject.Application.Features.Queries.Reservation;
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
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReservationController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<Guid>>> Create([FromBody] CreateReservationRequest request)
        {
            var result = await _mediator.Send(new CreateReservationCommand(request));
            return Ok(result);
        }

        [HttpPost("{id:guid}/confirm")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<string>>> Confirm([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new ConfirmReservationCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/checkin")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<string>>> CheckIn([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new CheckInReservationCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/complete")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<string>>> Complete([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new CompleteReservationCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/cancel")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<string>>> Cancel([FromRoute] Guid id, [FromQuery] string reason)
        {
            var result = await _mediator.Send(new CancelReservationCommand(id, reason));
            return Ok(result);
        }

        [HttpPost("{id:guid}/noshow")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<string>>> MarkNoShow([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new MarkNoShowReservationCommand(id));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<ReservationResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<ReservationResponse>>> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetReservationByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SimpleResponse<List<ReservationResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<ReservationResponse>>>> GetAll([FromQuery] DateTime? date = null, [FromQuery] CloPosProject.Domain.Enums.ReservationStatus? status = null)
        {
            var result = await _mediator.Send(new GetAllReservationsQuery(date, status));
            return Ok(result);
        }

        [HttpGet("table/{tableId:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<List<ReservationResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<ReservationResponse>>>> GetTableReservations([FromRoute] Guid tableId, [FromQuery] DateTime date)
        {
            var result = await _mediator.Send(new GetTableReservationsQuery(tableId, date));
            return Ok(result);
        }

        [HttpGet("available/{tableId:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<List<AvailableTimeSlot>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<AvailableTimeSlot>>>> GetAvailable([FromRoute] Guid tableId, [FromQuery] DateTime date, [FromQuery] int durationMinutes)
        {
            var result = await _mediator.Send(new GetAvailableTimeSlotsQuery(tableId, date, durationMinutes));
            return Ok(result);
        }
    }
}
