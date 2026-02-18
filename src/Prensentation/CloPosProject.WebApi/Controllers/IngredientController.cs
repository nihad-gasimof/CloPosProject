using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Ingredient;
using CloPosProject.Application.Features.Commands.Ingredient;
using CloPosProject.Application.Features.Queries.Ingredient;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CloPosProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IngredientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<Guid>>> Create([FromBody] CreateIngredientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateIngredientCommand(dto));
            return Ok(result);
        }

        [HttpPost("{id:guid}/add-stock")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> AddStock([FromRoute] Guid id, [FromQuery] decimal quantity, [FromQuery] decimal unitPrice)
        {
            var result = await _mediator.Send(new AddStockCommand(id, quantity, unitPrice));
            return Ok(result);
        }

        [HttpPost("{id:guid}/use-stock")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> UseStock([FromRoute] Guid id, [FromQuery] decimal quantity)
        {
            var result = await _mediator.Send(new UseStockCommand(id, quantity));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<IngredientResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<IngredientResponseDto>>> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetIngredientByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SimpleResponse<List<IngredientResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<IngredientResponseDto>>>> GetAll([FromQuery] bool? isActive)
        {
            var result = await _mediator.Send(new GetAllIngredientsQuery(isActive));
            return Ok(result);
        }

        [HttpGet("low-stock")]
        [ProducesResponseType(typeof(SimpleResponse<List<LowStockResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleResponse<List<LowStockResponseDto>>>> GetLowStock()
        {
            var result = await _mediator.Send(new GetLowStockIngredientsQuery());
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<string>>> Update([FromRoute] Guid id, [FromBody] UpdateIngredientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateIngredientCommand(id, dto));
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> SoftDelete([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteIngredientCommand(id));
            return Ok(result);
        }
    }
}
