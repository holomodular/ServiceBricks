namespace ServiceBricks
{
    /// <summary>
    /// This is the base class that all domain objects inherit from.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public abstract class DomainObject<TDomainObject> : IDomainObject<TDomainObject>
    {
    }
}