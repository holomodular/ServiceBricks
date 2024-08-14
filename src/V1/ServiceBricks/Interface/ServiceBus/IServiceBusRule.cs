namespace ServiceBricks
{
    /// <summary>
    /// This is a service bus rule used to handle messages from the service bus.
    /// </summary>
    public partial interface IServiceBusRule
    {
    }

    /// <summary>
    /// This is a service bus rule used to handle messages from the service bus.
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    public partial interface IServiceBusRule<TObj> : IServiceBusRule
    {
        /// <summary>
        /// Handle a service bus message.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task HandleServiceBusMessage(TObj obj);
    }
}