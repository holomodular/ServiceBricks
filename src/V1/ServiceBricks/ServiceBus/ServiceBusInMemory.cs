namespace ServiceBricks
{
    public class ServiceBusInMemory : IServiceBus
    {
        private readonly IServiceBusQueue _serviceBusQueue;
        private readonly IBusinessRuleRegistry _domainRuleRegistry;

        public ServiceBusInMemory(
            IServiceBusQueue serviceBusQueue,
            IBusinessRuleRegistry domainRuleRegistry)
        {
            _serviceBusQueue = serviceBusQueue;
            _domainRuleRegistry = domainRuleRegistry;
        }

        public void Start()
        {
        }

        public virtual Task StartAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void Send(IDomainBroadcast message)
        {
            var detail = new ServiceBusTask<IDomainBroadcast>.Detail(message);
            _serviceBusQueue.Queue(detail);
        }

        public virtual Task SendAsync(IDomainBroadcast message)
        {
            Send(message);
            return Task.CompletedTask;
        }

        public virtual void Subscribe(Type message, Type handler)
        {
            _domainRuleRegistry.RegisterItem(message, handler);
        }

        public virtual Task SubscribeAsync(Type message, Type handler)
        {
            Subscribe(message, handler);
            return Task.CompletedTask;
        }

        public virtual void Unsubscribe(Type message, Type handler)
        {
            _domainRuleRegistry.UnRegisterItem(message, handler);
        }

        public virtual Task UnsubscribeAsync(Type message, Type handler)
        {
            Unsubscribe(message, handler);
            return Task.CompletedTask;
        }
    }
}