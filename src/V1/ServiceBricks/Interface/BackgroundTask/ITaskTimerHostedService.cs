namespace ServiceBricks
{
    /// <summary>
    /// This is a hosted service using a timer to schedule processing.
    /// </summary>
    /// <typeparam name="TWorkDetail"></typeparam>
    /// <typeparam name="TWorker"></typeparam>
    public interface ITaskTimerHostedService<TWorkDetail, TWorker>
            where TWorker : ITaskWork<TWorkDetail, TWorker>
            where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
    {
        TimeSpan TimerTickInterval { get; }
        TimeSpan TimerDueTime { get; }
        ITaskDetail<TWorkDetail, TWorker> TaskDetail { get; }

        bool TimerTickShouldProcessRun();
    }
}