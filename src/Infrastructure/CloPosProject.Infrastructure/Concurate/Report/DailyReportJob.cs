using CloPosProject.Application.Abstract.Report;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Infrastructure.Concurate.Report
{
    public class DailyReportJob
    {
        private readonly  IReportService _reportService;
        public DailyReportJob(IReportService reportService)
        {
            _reportService = reportService;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task GenerateYesterdayReport()
        {
            

            var yesterday = DateTime.Today.AddDays(-1);


            var result = await _reportService.GenerateDailyReportAsync(yesterday);

            if (result.Success)
            {
            }
        }
    }
}
