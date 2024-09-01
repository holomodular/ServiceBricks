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

        protected Task _backgroundTask = null;
        protected int _shutdownRequested = 0;

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
            Interlocked.Exchange(ref _shutdownRequested, 0);
            _backgroundTask = Task.Run(async () =>
            {
                await BackgroundProcessAsync(cancellationToken);
            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops the background task.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Interlocked.Exchange(ref _shutdownRequested, 1);
            return Task.CompletedTask;
        }

        /// <summary>
        /// The background process that dequeues work and processes it.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task BackgroundProcessAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _shutdownRequested == 0)
            {
                try
                {
                    var workDetail = await _backgroundTaskQueue.DequeueAsync(cancellationToken);
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
                            .Invoke(worker, new object[] { workDetail, cancellationToken });
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