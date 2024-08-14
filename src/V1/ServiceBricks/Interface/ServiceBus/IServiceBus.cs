namespace ServiceBricks
{
    /// <summary>
    /// This is a service bus used to relay messages between domains.
    /// </summary>
    public partial interface IServiceBus
    {
        /// <summary>
        /// Start processing messages.
        /// </summary>
        void Start();

        /// <summary>
        /// Start processing messages.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();

        /// <summary>
        /// Send a message to the service bus.
        /// </summary>
        /// <param name="message"></param>
        void Send(IDomainBroadcast message);

        /// <summary>
        /// Send a message to the service bus.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendAsync(IDomainBroadcast message);

        /// <summary>
        /// Subscribe to a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        void Subscribe(Type message, Type handler);

        /// <summary>
        /// Subscribe to a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        Task SubscribeAsync(Type message, Type handler);

        /// <summary>
        /// Unsubscribe from a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        void Unsubscribe(Type message, Type handler);

        /// <summary>
        /// Unsubscribe from a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        Task UnsubscribeAsync(Type message, Type handler);
    }
}