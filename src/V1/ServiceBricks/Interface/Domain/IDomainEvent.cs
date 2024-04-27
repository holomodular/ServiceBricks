namespace ServiceBricks
{
    /// <summary>
    /// This is an event.
    /// </summary>
    public interface IDomainEvent
    {
    }

    /// <summary>
    /// This is an event.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public interface IDomainEvent<TDomainObject> : IDomainEvent
    {
        TDomainObject DomainObject { get; set; }
    }
}