using System.Collections.Concurrent;

namespace ServiceBricks
{
    /// <summary>
    /// This queues work to be processed in order on a background task.
    /// </summary>
    public partial class TaskQueue : ITaskQueue
    {
        protected readonly ConcurrentQueue<ITaskDetail> _queue = new ConcurrentQueue<ITaskDetail>();
        protected readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <typeparam name="TWorkDetail"></typeparam>
        /// <typeparam name="TWorker"></typeparam>
        /// <param name="detail"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Queue<TWorkDetail, TWorker>(ITaskDetail<TWorkDetail, TWorker> detail)
            where TWorker : ITaskWork<TWorkDetail, TWorker>
            where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));
            _queue.Enqueue(detail);
            _signal.Release();
        }

        /// <summary>
        /// Dequeues the next work item from the queue.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<ITaskDetail> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var workDetail);
            return workDetail;
        }
    }
}