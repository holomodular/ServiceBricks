using System.Collections.Concurrent;

namespace ServiceBricks
{
    /// <summary>
    /// This queues work to be processed in order on a background task.
    /// </summary>
    public class TaskQueue : ITaskQueue
    {
        private readonly ConcurrentQueue<ITaskDetail> _queue = new ConcurrentQueue<ITaskDetail>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void Queue<TWorkDetail, TWorker>(ITaskDetail<TWorkDetail, TWorker> detail)
            where TWorker : ITaskWork<TWorkDetail, TWorker>
            where TWorkDetail : ITaskDetail<TWorkDetail, TWorker>
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));
            _queue.Enqueue(detail);
            _signal.Release();
        }

        public async Task<ITaskDetail> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var workDetail);
            return workDetail;
        }
    }
}