namespace ServiceBricks
{
    /// <summary>
    /// This is an event.
    /// </summary>
    public interface IDomainBroadcast
    {
    }

    /// <summary>
    /// This is an event.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public interface IDomainBroadcast<TDomainObject> : IDomainBroadcast
    {
        TDomainObject DomainObject { get; set; }
    }
}