using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger _logger = Log.ForContext<QueuedHostedService>();

        public QueuedHostedService(IBackgroundTaskQueue taskQueue)
        {
            TaskQueue = taskQueue;
        }

        public IBackgroundTaskQueue TaskQueue { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Queued Hosted Service is running.");

            await BackgroundProcessing(stoppingToken);
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(stoppingToken);

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
