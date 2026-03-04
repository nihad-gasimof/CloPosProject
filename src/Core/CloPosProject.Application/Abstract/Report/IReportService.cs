using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Application.DTOs.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Report
{
    public interface IReportService
    {
        Task<SimpleResponse<string>> GenerateDailyReportAsync(DateTime date);
        Task<SimpleResponse<DailyReportResponse>> GetDailyReportAsync(DateTime date);
        Task<SimpleResponse<List<DailyReportSummary>>> GetReportRangeAsync(DateTime startDate, DateTime endDate);
        Task<SimpleResponse<MonthlyReportResponse>> GetMonthlyReportAsync(int year, int month);
    }
}
