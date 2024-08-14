namespace ServiceBricks
{
    /// <summary>
    /// This is an inter-process event raised in the platform. Handled by Service Bus.
    /// </summary>
    public partial class DomainBroadcast : IDomainBroadcast<object>, IDomainBroadcast
    {
        /// <summary>
        /// The object that is being broadcasted.
        /// </summary>
        public virtual object DomainObject { get; set; }
    }

    /// <summary>
    /// This is an inter-process event raised in the platform based on a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainBroadcast<TDomainObject> : IDomainBroadcast<TDomainObject>, IDomainBroadcast
    {
        /// <summary>
        /// The object that is being broadcasted.
        /// </summary>
        public virtual TDomainObject DomainObject { get; set; }
    }
}