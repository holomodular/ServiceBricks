namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// This storage repository uses the Entity Framework Core provider.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public interface IEntityFrameworkCoreStorageRepository<TDomain> : IDbStorageRepository<TDomain>
        where TDomain : class, IEntityFrameworkCoreDomainObject<TDomain>, new()
    {
        IQueryable<TDomain> GetQueryable();
    }
}