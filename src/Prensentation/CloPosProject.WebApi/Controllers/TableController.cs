using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Table;
using CloPosProject.Application.Features.Commands.Table;
using CloPosProject.Application.Features.Queries.Table;
using CloPosProject.Domain.Enums;
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
    public class TableController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TableController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<Guid>>> Create([FromBody] CloPosProject.Application.DTOs.Table.CreateTableRequest dto)
        {
            var result = await _mediator.Send(new CreateTableCommand(dto.TableNumber, dto.Capacity, dto.Location));
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<string>>> Update([FromRoute] Guid id, [FromBody] UpdateTableRequest dto)
        {
            var result = await _mediator.Send(new UpdateTableCommand(id, dto.TableNumber, dto.Capacity, dto.Location));
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> Delete([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteTableCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/activate")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> Activate([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new ActivateTableCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/deactivate")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> Deactivate([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeactivateTableCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/status")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<string>>> ChangeStatus([FromRoute] Guid id, [FromQuery] TableStatus status)
        {
            var result = await _mediator.Send(new ChangeTableStatusCommand(id, status));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<TableResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<TableResponse>>> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetTableByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SimpleResponse<List<TableSummaryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<TableSummaryResponse>>>> GetAll([FromQuery] bool? isActive, [FromQuery] TableStatus? status)
        {
            var result = await _mediator.Send(new GetAllTablesQuery(isActive, status));
            return Ok(result);
        }
    }
}
