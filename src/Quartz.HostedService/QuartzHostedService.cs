using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Quartz.HostedService
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IScheduler _scheduler;

        public QuartzHostedService(ILogger<QuartzHostedService> logger, IScheduler scheduler)
        {
            _logger = logger;
            _scheduler = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Quartz started...");
            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Quartz stopped...");
            await _scheduler.Shutdown(cancellationToken);
        }
    }
}