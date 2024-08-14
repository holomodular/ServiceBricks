namespace ServiceBricks
{
    /// <summary>
    /// This queues work to be processed in order on a background task.
    /// </summary>
    public partial interface ITaskQueue
    {
        /// <summary>
        /// Queue work to be processed.
        /// </summary>
        /// <typeparam name="TWorkDetail"></typeparam>
        /// <typeparam name="TWorker"></typeparam>
        /// <param name="order"></param>
        void Queue<TWorkDetail, TWorker>(ITaskDetail<TWorkDetail, TWorker> order)
            where TWorker : ITaskWork<TWorkDetail, TWorker>
            where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>;

        /// <summary>
        /// Dequeue work to be processed.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ITaskDetail> DequeueAsync(CancellationToken cancellationToken);
    }
}