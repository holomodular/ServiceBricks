using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a hosted service that executes work on a queue.
    /// </summary>
    public class TaskQueueHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TaskQueueHostedService> _logger;
        private readonly ITaskQueue _backgroundTaskQueue;
        private CancellationTokenSource _shutdown;

        private Task _backgroundTask = null;

        public TaskQueueHostedService(
            IServiceProvider serviceProvider,
            ITaskQueue backgroundTaskQueue,
            ILoggerFactory logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger.CreateLogger<TaskQueueHostedService>();
            _backgroundTaskQueue = backgroundTaskQueue;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _shutdown = new CancellationTokenSource();
            _backgroundTask = Task.Run(this.BackgroundProcess);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _shutdown?.Cancel();
            return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        protected virtual async Task BackgroundProcess()
        {
            while (!_shutdown.IsCancellationRequested)
            {
                var workDetail = await _backgroundTaskQueue.DequeueAsync(_shutdown.Token);

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var workerType = workDetail
                            .GetType()
                            .GetInterfaces()
                            .First(x => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == typeof(ITaskDetail<,>))
                            .GetGenericArguments()
                            .Last();

                        var worker = scope.ServiceProvider.GetRequiredService(workerType);
                        var task = (Task)workerType.GetMethod("DoWork")
                            .Invoke(worker, new object[] { workDetail, _shutdown.Token });
                        await task;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, ex.Message);
                }
            }
        }
    }
}