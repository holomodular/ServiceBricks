namespace ServiceBricks
{
    /// <summary>
    /// This is the base repository interface.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public partial interface IDomainRepository<TDomainObject> : IRepository<TDomainObject>
        where TDomainObject : class
    {
        /// <summary>
        /// Get the underlying storage repository.
        /// </summary>
        /// <returns></returns>
        IStorageRepository<TDomainObject> GetStorageRepository();
    }
}