namespace ServiceBricks
{
    /// <summary>
    /// This is the storage repository interface.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial interface IStorageRepository<TDomain> : IDomainRepository<TDomain>
        where TDomain : class
    {
    }
}