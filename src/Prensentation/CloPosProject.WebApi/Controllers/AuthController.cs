using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.Features.Commands.User;
using CloPosProject.Application.DTOs.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CloPosProject.Domain.Enums;
using CloPosProject.Application.Features.Queries.User;

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
            if (result.IsSuccess)
            {
            return Ok(result);
                
            }
            return StatusCode(result.StatusCode, result);
        }
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            var result = await _mediator.Send(new RegisterCommand(dto));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [HttpPost("AssignRole/{Id}")]
        public async Task<IActionResult> AssignRole([FromRoute] Guid Id ,[FromQuery]Roles role)
        {
            var result = await _mediator.Send(new AssignedRoleCommand(Id,role));
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _mediator.Send(new GetAllUserQuery());
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("İstifadəçi ID və ya Token çatışmır.");
            }

            var result = await _mediator.Send(new ConfirmEmailCommand(userId,token));

            if (result.IsSuccess)
            {
                return Ok("Hesabınız uğurla təsdiqləndi!");
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }
        [HttpPost("SeedRole")]
        public async Task<SimpleResponse<string>> SeedRole()
        {
            var roles = Enum.GetNames(typeof(Domain.Enums.Roles));
            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            return new SimpleResponse<string>("Ugurla rollar yaradildi", roles.First());
        }
        //[ProducesResponseType(typeof(Response<AuthResponseDto>), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(Response<AuthResponseDto>), StatusCodes.Status400BadRequest)]
        //[HttpPost("RefreshToken")]
        //public async Task<IActionResult> RefreshToken([FromForm] string token)
        //{
        //    var result = await _mediator.Send(new RefreshTokenCommand(token));
        //    return Ok(result);

        //}
    }
}