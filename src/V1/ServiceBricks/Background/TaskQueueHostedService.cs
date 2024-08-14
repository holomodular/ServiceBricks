using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a hosted service that executes work on a queue.
    /// </summary>
    public partial class TaskQueueHostedService : IHostedService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger<TaskQueueHostedService> _logger;
        protected readonly ITaskQueue _backgroundTaskQueue;

        protected CancellationTokenSource _shutdown;
        protected Task _backgroundTask = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="backgroundTaskQueue"></param>
        /// <param name="logger"></param>
        public TaskQueueHostedService(
            IServiceProvider serviceProvider,
            ITaskQueue backgroundTaskQueue,
            ILoggerFactory logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger.CreateLogger<TaskQueueHostedService>();
            _backgroundTaskQueue = backgroundTaskQueue;
        }

        /// <summary>
        /// Starts the background task.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _shutdown = new CancellationTokenSource();
            _backgroundTask = Task.Run(this.BackgroundProcess);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops the background task.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            _shutdown?.Cancel();
            return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        /// <summary>
        /// The background process that dequeues work and processes it.
        /// </summary>
        /// <returns></returns>
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