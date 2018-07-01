using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Quartz.HostedService.Sample
{
    public class TestJob : IJob
    {
        private readonly ILogger _logger;

        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation(string.Format("[{0:yyyy-MM-dd hh:mm:ss:ffffff}]Test job is running...", DateTime.Now));
            return Task.CompletedTask;
        }
    }
}