namespace ServiceBricks.Storage.AzureDataTables
{
    /// <summary>
    /// This storage repository uses the Entity Framework ORM to provide database specific functions.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public interface IAzureDataTablesStorageRepository<TDomain> : IStorageRepository<TDomain>
        where TDomain : class, IAzureDataTablesDomainObject<TDomain>, new()
    {
    }
}