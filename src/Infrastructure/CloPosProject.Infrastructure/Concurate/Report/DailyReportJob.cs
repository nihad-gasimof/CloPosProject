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


        [AutomaticRetry(Attempts = 3)]
        public async Task GenerateYesterdayReport(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

            var yesterday = DateTime.Today.AddDays(-1);


            var result = await reportService.GenerateDailyReportAsync(yesterday);

            if (result.Success)
            {
            }
        }
    }
}
