namespace ServiceBricks
{
    /// <summary>
    /// This queues work to be processed in order on a background task.
    /// </summary>
    public partial class ServiceBusQueue : TaskQueue, IServiceBusQueue
    {
    }
}