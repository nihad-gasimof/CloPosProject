using CloPosProject.Application.Abstract.Report;
using CloPosProject.Infrastructure.Concurate.Report;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloPosProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateReport([FromQuery] DateTime? date = null)
        {
            var reportDate = date ?? DateTime.Today.AddDays(-1);
            var result = await _reportService.GenerateDailyReportAsync(reportDate);

            if (!result.Success)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyReport([FromQuery] DateTime? date = null)
        {
            var reportDate = date ?? DateTime.Today.AddDays(-1);
            var result = await _reportService.GetDailyReportAsync(reportDate);

            if (!result.Success)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetReportRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var result = await _reportService.GetReportRangeAsync(startDate, endDate);

            if (!result.Success)
                return BadRequest(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyReport(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var result = await _reportService.GetMonthlyReportAsync(year, month);

            if (!result.Success)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpPost("trigger-now")]
        public IActionResult TriggerReportNow()
        {
            BackgroundJob.Enqueue<DailyReportJob>(job =>
                job.GenerateYesterdayReport());

            return Ok(new { message = "Hesabat prosesi işə salındı" });
        }
    }
}
