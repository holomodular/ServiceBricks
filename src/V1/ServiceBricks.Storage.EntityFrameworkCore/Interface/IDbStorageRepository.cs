using Microsoft.EntityFrameworkCore;

namespace ServiceBricks.Storage.EntityFrameworkCore
{
    /// <summary>
    /// This is the storage repository for databases.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial interface IDbStorageRepository<TDomain> : IStorageRepository<TDomain>
        where TDomain : class
    {
        /// <summary>
        /// The database set.
        /// </summary>
        public DbSet<TDomain> DbSet { get; set; }

        /// <summary>
        /// The database context.
        /// </summary>
        public DbContext Context { get; set; }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <returns></returns>
        IResponse SaveChanges();

        /// <summary>
        /// Save changes asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<IResponse> SaveChangesAsync();
    }
}