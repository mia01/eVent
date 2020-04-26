using eventapp.Domain.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventNotification
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _services;

        public Worker(ILogger<Worker> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                using (var scope = _services.CreateScope())
                {
                    var job = scope.ServiceProvider.GetRequiredService<ReminderNotificationJob>();

                    try
                    {
                        await job.Execute();
                    } catch (Exception e)
                    {
                        _logger.LogInformation("An exception ocurred: ", e.Message);
                    }
                }
                await Task.Delay(6000, stoppingToken);
            }
        }
    }
}
