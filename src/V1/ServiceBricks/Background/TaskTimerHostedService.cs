using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServiceBricks
{
    /// <summary>
    /// This is a hosted service that executes work using a timer.
    /// </summary>
    /// <typeparam name="TWorkDetail"></typeparam>
    /// <typeparam name="TWorker"></typeparam>
    public abstract partial class TaskTimerHostedService<TWorkDetail, TWorker> : IHostedService, ITaskTimerHostedService<TWorkDetail, TWorker>
        where TWorker : ITaskWork<TWorkDetail, TWorker>
        where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger _logger = null;

        protected CancellationTokenSource _shutdown;
        protected Timer _timer;
        protected Task _backgroundTask = null;
        protected bool _isCurrentlyRunning = false;

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
        /// Indicates if the timer is currently running.
        /// </summary>
        public virtual bool IsCurrentlyRunning
        { get { return _isCurrentlyRunning; } }

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
            _shutdown = new CancellationTokenSource();
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
            _shutdown?.Cancel();
            var taskList = new List<Task>()
            {
                Task.Delay(Timeout.Infinite,cancellationToken)
            };
            if (_backgroundTask != null)
                taskList.Add(_backgroundTask);
            return Task.WhenAny(taskList);
        }

        /// <summary>
        /// The timer processing method.
        /// </summary>
        /// <param name="state"></param>
        protected virtual void TimerProcessing(object state)
        {
            if (TimerTickShouldProcessRun())
                Task.Run(async () => await BackgroundProcess());
        }

        /// <summary>
        /// The background process that dequeues work and processes it.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task BackgroundProcess()
        {
            try
            {
                _isCurrentlyRunning = true;
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
                        .Invoke(worker, new object[] { TaskDetail, _shutdown.Token });
                    await task;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
            }
            finally
            {
                _isCurrentlyRunning = false;
            }
        }
    }
}