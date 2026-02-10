using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Commands;
using CloPosProject.Application.DTOs.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CloPosProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(IMediator mediator, RoleManager<IdentityRole> roleManager)
        {
            _mediator = mediator;
            _roleManager = roleManager;
        }
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto));
            return Ok(result);
        }
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            var result = await _mediator.Send(new RegisterCommand(dto));
            return Ok(result);
        }
        //[HttpPost("SeedRole")]
        //public async Task<SimpleResponse<string>> SeedRole()
        //{
        //    var roles=Enum.GetNames(typeof(Domain.Enums.Roles));
        //    foreach (var role in roles)
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole(role));           
        //    }
        //    return new SimpleResponse<string>("Ugurla rollar yaradildi",roles.First());
        //}

    }
}