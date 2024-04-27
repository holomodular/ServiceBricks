namespace ServiceBricks
{
    /// <summary>
    /// This is the base interface that all domain objects inherit from.
    /// </summary>
    public interface IDomainObject
    {
    }

    /// <summary>
    /// This is the base interface that all domain objects inherit from.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDomainObject<T> : IDomainObject
    {
    }
}