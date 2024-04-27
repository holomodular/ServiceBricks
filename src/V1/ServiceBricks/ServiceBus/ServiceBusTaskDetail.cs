namespace ServiceBricks
{
    public class ServiceBusTaskDetail<TWorker, TBroadcast> : ITaskDetail<ServiceBusTaskDetail<TWorker, TBroadcast>, TWorker>
        where TWorker : ITaskWork<ServiceBusTaskDetail<TWorker, TBroadcast>, TWorker>
    {
        public ServiceBusTaskDetail(TBroadcast message)
        {
            Message = message;
        }

        public virtual TBroadcast Message { get; set; }
    }
}