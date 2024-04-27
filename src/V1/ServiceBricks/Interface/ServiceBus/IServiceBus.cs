namespace ServiceBricks
{
    /// <summary>
    /// This is a service bus used to relay messages between domains.
    /// </summary>
    public interface IServiceBus
    {
        void Start();

        Task StartAsync();

        void Send(IDomainBroadcast message);

        Task SendAsync(IDomainBroadcast message);

        void Subscribe(Type message, Type handler);

        Task SubscribeAsync(Type message, Type handler);

        void Unsubscribe(Type message, Type handler);

        Task UnsubscribeAsync(Type message, Type handler);
    }
}