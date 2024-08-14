namespace ServiceBricks
{
    /// <summary>
    /// This is an event.
    /// </summary>
    public partial interface IDomainEvent
    {
    }

    /// <summary>
    /// This is an event.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial interface IDomainEvent<TDomainObject> : IDomainEvent
    {
        TDomainObject DomainObject { get; set; }
    }
}