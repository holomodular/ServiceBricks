namespace ServiceBricks
{
    /// <summary>
    /// This is an event that is sent over the service bus provider.
    /// </summary>
    public partial interface IDomainBroadcast
    {
    }

    /// <summary>
    /// This is an event that is sent over the service bus provider.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial interface IDomainBroadcast<TDomainObject> : IDomainBroadcast
    {
        /// <summary>
        /// The domain object.
        /// </summary>
        TDomainObject DomainObject { get; set; }
    }
}