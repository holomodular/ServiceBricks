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
    public abstract class TaskTimerHostedService<TWorkDetail, TWorker> : IHostedService, ITaskTimerHostedService<TWorkDetail, TWorker>
        where TWorker : ITaskWork<TWorkDetail, TWorker>
        where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger _logger = null;

        protected CancellationTokenSource _shutdown;
        protected Timer _timer;
        protected Task _backgroundTask = null;
        protected bool _isCurrentlyRunning = false;

        public TaskTimerHostedService(
            IServiceProvider serviceProvider,
            ILoggerFactory logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger.CreateLogger<TaskTimerHostedService<TWorkDetail, TWorker>>();
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }

        public virtual bool IsCurrentlyRunning
        { get { return _isCurrentlyRunning; } }

        public abstract TimeSpan TimerTickInterval { get; }

        public abstract TimeSpan TimerDueTime { get; }
        public abstract ITaskDetail<TWorkDetail, TWorker> TaskDetail { get; }

        public abstract bool TimerTickShouldProcessRun();

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _shutdown = new CancellationTokenSource();
            _timer = new Timer(TimerProcessing, null, TimerDueTime, TimerTickInterval);
            return Task.CompletedTask;
        }

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

        protected virtual void TimerProcessing(object state)
        {
            if (TimerTickShouldProcessRun())
                Task.Run(async () => await BackgroundProcess());
        }

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