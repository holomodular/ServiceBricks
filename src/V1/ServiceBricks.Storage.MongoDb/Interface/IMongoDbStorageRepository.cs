namespace ServiceBricks.Storage.MongoDb
{
    /// <summary>
    /// This storage repository uses the MongoDb provider.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public interface IMongoDbStorageRepository<TDomain> : IStorageRepository<TDomain>
        where TDomain : class, IMongoDbDomainObject<TDomain>, new()
    {
    }
}