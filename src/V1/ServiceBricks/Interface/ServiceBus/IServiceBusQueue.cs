namespace ServiceBricks
{
    /// <summary>
    /// This queues service bus messages to be processed in order on a background task.
    /// </summary>
    public interface IServiceBusQueue : ITaskQueue
    {
    }
}