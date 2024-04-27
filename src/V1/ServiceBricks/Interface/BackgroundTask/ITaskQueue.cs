namespace ServiceBricks
{
    /// <summary>
    /// This queues work to be processed in order on a background task.
    /// </summary>
    public interface ITaskQueue
    {
        void Queue<TWorkDetail, TWorker>(ITaskDetail<TWorkDetail, TWorker> order)
            where TWorker : ITaskWork<TWorkDetail, TWorker>
            where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>;

        Task<ITaskDetail> DequeueAsync(CancellationToken cancellationToken);
    }
}