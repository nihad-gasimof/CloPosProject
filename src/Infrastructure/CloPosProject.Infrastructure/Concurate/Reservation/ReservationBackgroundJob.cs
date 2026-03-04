using CloPosProject.Application.Abstract.Reservation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Infrastructure.Concurate.Reservation
{
    public class ReservationBackgroundJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public ReservationBackgroundJob(
               IServiceProvider serviceProvider,
               ILogger<ReservationBackgroundJob> logger)
        {
            _serviceProvider = serviceProvider;
        }
          protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                    using var scope = _serviceProvider.CreateScope();
                    var reservationService = scope.ServiceProvider
                        .GetRequiredService<IReservationService>();

                    await reservationService.ProcessExpiredReservationsAsync();


                await Task.Delay(_interval, stoppingToken);
            }

        }
    }
}
