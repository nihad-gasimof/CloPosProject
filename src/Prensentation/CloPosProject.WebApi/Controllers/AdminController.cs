using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CloPosProject.Application.Abstract.Ai;
using CloPosProject.Application.BaseResponseModel;

[Route("api/[controller]")]
[ApiController]

public class AdminAIController : ControllerBase
{
    private readonly IAdminAIService _adminAiService;

    public AdminAIController(IAdminAIService adminAiService)
    {
        _adminAiService = adminAiService;
    }

    [HttpPost("ask-assistant")]
    public async Task<IActionResult> AskAssistant([FromBody] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Sual boş ola bilməz.");

        var response = await _adminAiService.ProcessAdminRequestAsync(query);

        if (response.Success)
            return Ok(response);

        return StatusCode(500, response);
    }
}