using Microsoft.EntityFrameworkCore;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// This is the storage repository for databases.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public interface IDbStorageRepository<TDomain> : IStorageRepository<TDomain>
        where TDomain : class
    {
        public DbSet<TDomain> DbSet { get; set; }
        public DbContext Context { get; set; }

        IResponse SaveChanges();

        Task<IResponse> SaveChangesAsync();
    }
}