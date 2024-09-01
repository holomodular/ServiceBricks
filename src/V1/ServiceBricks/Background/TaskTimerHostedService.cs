using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace ServiceBricks
{
    /// <summary>
    /// This is a hosted service that executes work using a timer.
    /// </summary>
    /// <typeparam name="TWorkDetail"></typeparam>
    /// <typeparam name="TWorker"></typeparam>
    public abstract partial class TaskTimerHostedService<TWorkDetail, TWorker> : IHostedService, ITaskTimerHostedService<TWorkDetail, TWorker>, IDisposable
        where TWorker : ITaskWork<TWorkDetail, TWorker>
        where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger _logger = null;

        protected Timer _timer;
        protected Task _backgroundTask = null;
        protected CancellationToken _cancellationToken;
        protected int _isCurrentlyRunning = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public TaskTimerHostedService(
            IServiceProvider serviceProvider,
            ILoggerFactory logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger.CreateLogger<TaskTimerHostedService<TWorkDetail, TWorker>>();
        }

        /// <summary>
        /// Disposes of the timer.
        /// </summary>
        public virtual void Dispose()
        {
            _timer?.Dispose();
        }

        /// <summary>
        /// Indicates if a timer process is currently running.
        /// </summary>
        public virtual bool IsCurrentlyRunning
        { get { return _isCurrentlyRunning == 1; } }

        /// <summary>
        /// The interval at which the timer ticks.
        /// </summary>
        public abstract TimeSpan TimerTickInterval { get; }

        /// <summary>
        /// The time to wait before the timer starts.
        /// </summary>
        public abstract TimeSpan TimerDueTime { get; }

        /// <summary>
        /// The work detail to be processed.
        /// </summary>
        public abstract ITaskDetail<TWorkDetail, TWorker> TaskDetail { get; }

        /// <summary>
        /// Indicates if the timer should process the work.
        /// </summary>
        /// <returns></returns>
        public abstract bool TimerTickShouldProcessRun();

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _timer = new Timer(TimerProcessing, null, TimerDueTime, TimerTickInterval);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// The timer processing method.
        /// </summary>
        /// <param name="state"></param>
        protected virtual void TimerProcessing(object state)
        {
            if (TimerTickShouldProcessRun())
            {
                // Start the background task
                Task.Run(async () =>
                {
                    await BackgroundProcess(_cancellationToken);
                });
            }
        }

        /// <summary>
        /// The background process that dequeues work and processes it.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task BackgroundProcess(CancellationToken cancellationToken)
        {
            // Mark the task as running
            Interlocked.Exchange(ref _isCurrentlyRunning, 1);

            // Create a scope for the worker
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var workerType = TaskDetail
                        .GetType()
                        .GetInterfaces()
                        .First(x => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == typeof(ITaskDetail<,>))
                        .GetGenericArguments()
                        .Last();

                    var worker = scope.ServiceProvider.GetRequiredService(workerType);
                    var task = (Task)workerType.GetMethod("DoWork")
                        .Invoke(worker, new object[] { TaskDetail, cancellationToken });
                    await task;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
            }

            // Mark the task as not running
            Interlocked.Exchange(ref _isCurrentlyRunning, 0);
        }
    }
}