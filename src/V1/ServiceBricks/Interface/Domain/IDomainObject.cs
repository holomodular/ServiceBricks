namespace ServiceBricks
{
    /// <summary>
    /// This is the base interface that all domain objects inherit from.
    /// </summary>
    public partial interface IDomainObject
    {
    }

    /// <summary>
    /// This is the base interface that all domain objects inherit from.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IDomainObject<T> : IDomainObject
    {
    }
}