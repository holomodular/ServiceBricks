namespace ServiceBricks
{
    /// <summary>
    /// This is an intra-process event raised in the platform.
    /// </summary>
    public partial class DomainEvent : IDomainEvent<object>, IDomainEvent
    {
        public virtual object DomainObject { get; set; }
    }

    /// <summary>
    /// This is an intra-process event raised in the platform based on a domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial class DomainEvent<TDomainObject> : IDomainEvent<TDomainObject>, IDomainEvent
    {
        public virtual TDomainObject DomainObject { get; set; }
    }
}