namespace ServiceBricks
{
    /// <summary>
    /// The detail of the task.
    /// </summary>
    /// <typeparam name="TWorker"></typeparam>
    /// <typeparam name="TBroadcast"></typeparam>
    public partial class ServiceBusTaskDetail<TWorker, TBroadcast> : ITaskDetail<ServiceBusTaskDetail<TWorker, TBroadcast>, TWorker>
        where TWorker : ITaskWork<ServiceBusTaskDetail<TWorker, TBroadcast>, TWorker>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public ServiceBusTaskDetail(TBroadcast message)
        {
            Message = message;
        }

        /// <summary>
        /// The message to process.
        /// </summary>
        public virtual TBroadcast Message { get; set; }
    }
}