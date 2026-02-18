using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.MenuItem;
using CloPosProject.Application.Features.Commands.MenuItem;
using CloPosProject.Application.Features.Queries.MenuItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloPosProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MenuItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<Guid>>> Create([FromForm] CreateMenuItem dto)
        {
            var result = await _mediator.Send(new CreateMenuItemCommand(dto));
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<string>>> Update([FromRoute] Guid id, [FromForm] UpdateMenuItem dto)
        {
            var result = await _mediator.Send(new UpdateMenuItemCommand(id, dto));
            return Ok(result);
        }

        [HttpPost("{id:guid}/ingredients")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<string>>> UpdateIngredients([FromRoute] Guid id, [FromBody] List<MenuItemIngredientRequest> ingredients)
        {
            var result = await _mediator.Send(new UpdateMenuItemIngredientsCommand(id, ingredients));
            return Ok(result);
        }

        [HttpPost("{id:guid}/availability")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<string>>> SetAvailability([FromRoute] Guid id, [FromQuery] bool isAvailable)
        {
            var result = await _mediator.Send(new SetAvailabilityCommand(id, isAvailable));
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> Delete([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteMenuItemCommand(id));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<MenuItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<MenuItemResponse>>> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetMenuItemByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SimpleResponse<List<MenuItemSummaryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<MenuItemSummaryResponse>>>> GetAll([FromQuery] bool? isAvailable, [FromQuery] Guid? categoryId)
        {
            var result = await _mediator.Send(new GetAllMenuItemsQuery(isAvailable, categoryId));
            return Ok(result);
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(SimpleResponse<List<MenuItemSummaryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<MenuItemSummaryResponse>>>> GetAvailable()
        {
            var result = await _mediator.Send(new GetAvailableMenuItemsQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}/can-prepare")]
        [ProducesResponseType(typeof(SimpleResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<bool>>> CanPrepare([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new CheckCanBePreparedQuery(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/deduct")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<string>>> Deduct([FromRoute] Guid id, [FromQuery] int quantity)
        {
            var result = await _mediator.Send(new DeductIngredientsCommand(id, quantity));
            return Ok(result);
        }
    }
}
