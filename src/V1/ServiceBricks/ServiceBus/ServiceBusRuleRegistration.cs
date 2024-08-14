namespace ServiceBricks
{
    /// <summary>
    /// This queues work to be processed in order on a background task.
    /// </summary>
    /// <typeparam name="TBroadcast"></typeparam>
    public partial class ServiceBusRuleRegistration<TBroadcast> : DomainEvent<TBroadcast>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public ServiceBusRuleRegistration(TBroadcast obj) : base()
        {
            DomainObject = obj;
        }
    }
}