using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Category;
using CloPosProject.Application.Features.Commands.Category;
using CloPosProject.Application.Features.Queries.Category;
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
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<Guid>>> Create([FromBody] CloPosProject.Application.DTOs.Category.CreateCategoryRequest dto)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(dto.Name, dto.Description, dto.DisplayOrder));
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SimpleResponse<string>>> Update([FromRoute] Guid id, [FromBody] CloPosProject.Application.DTOs.Category.UpdateCategoryRequest dto)
        {
            var result = await _mediator.Send(new UpdateCategoryCommand(id, dto.Name, dto.Description, dto.DisplayOrder));
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> Delete([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/activate")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> Activate([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new ActivateCategoryCommand(id));
            return Ok(result);
        }

        [HttpPost("{id:guid}/deactivate")]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SimpleResponse<string>>> Deactivate([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeactivateCategoryCommand(id));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SimpleResponse<CategoryResponse>>> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<SimpleResponse<List<CategoryResponse>>>> GetAll([FromQuery] bool? isActive)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery(isActive));
            return Ok(result);
        }
    }
}
