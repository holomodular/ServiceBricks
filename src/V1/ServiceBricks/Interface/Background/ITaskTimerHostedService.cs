namespace ServiceBricks
{
    /// <summary>
    /// This is a hosted service using a timer to schedule processing.
    /// </summary>
    /// <typeparam name="TWorkDetail"></typeparam>
    /// <typeparam name="TWorker"></typeparam>
    public partial interface ITaskTimerHostedService<TWorkDetail, TWorker>
            where TWorker : ITaskWork<TWorkDetail, TWorker>
            where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
    {
        /// <summary>
        /// THe interval for the timer.
        /// </summary>
        TimeSpan TimerTickInterval { get; }

        /// <summary>
        /// The due time for the timer.
        /// </summary>
        TimeSpan TimerDueTime { get; }

        /// <summary>
        /// The task detail to be processed.
        /// </summary>
        ITaskDetail<TWorkDetail, TWorker> TaskDetail { get; }

        /// <summary>
        /// Determines if the timer should process the task.
        /// </summary>
        /// <returns></returns>
        bool TimerTickShouldProcessRun();
    }
}