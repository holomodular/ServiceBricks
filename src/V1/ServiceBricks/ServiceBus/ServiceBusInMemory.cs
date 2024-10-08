﻿namespace ServiceBricks
{
    /// <summary>
    /// In-memory implementation of the service bus.
    /// </summary>
    public partial class ServiceBusInMemory : IServiceBus
    {
        protected readonly IServiceBusQueue _serviceBusQueue;
        protected readonly IBusinessRuleRegistry _businessRuleRegistry;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceBusQueue"></param>
        /// <param name="businessRuleRegistry"></param>
        public ServiceBusInMemory(
            IServiceBusQueue serviceBusQueue,
            IBusinessRuleRegistry businessRuleRegistry)
        {
            _serviceBusQueue = serviceBusQueue;
            _businessRuleRegistry = businessRuleRegistry;
        }

        /// <summary>
        /// Start the service bus.
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Start the service bus asynchronously.
        /// </summary>
        /// <returns></returns>
        public virtual Task StartAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Send a message to the service bus.
        /// </summary>
        /// <param name="message"></param>
        public virtual void Send(IDomainBroadcast message)
        {
            var detail = new ServiceBusTask<IDomainBroadcast>.Detail(message);
            _serviceBusQueue.Queue(detail);
        }

        /// <summary>
        /// Sebd a message to the service bus asynchronously.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual Task SendAsync(IDomainBroadcast message)
        {
            Send(message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Subscribe to a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        public virtual void Subscribe(Type message, Type handler)
        {
            _businessRuleRegistry.Register(message, handler);
        }

        /// <summary>
        /// Subscribe to a message asynchronously.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public virtual Task SubscribeAsync(Type message, Type handler)
        {
            Subscribe(message, handler);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Unsubscribe from a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        public virtual void UnSubscribe(Type message, Type handler)
        {
            _businessRuleRegistry.UnRegister(message, handler);
        }

        /// <summary>
        /// Unsubscribe from a message asynchronously.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public virtual Task UnSubscribeAsync(Type message, Type handler)
        {
            UnSubscribe(message, handler);
            return Task.CompletedTask;
        }
    }
}