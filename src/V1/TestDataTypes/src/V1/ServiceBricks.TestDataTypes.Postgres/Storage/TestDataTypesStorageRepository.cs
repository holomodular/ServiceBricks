using Microsoft.Extensions.Logging;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.Postgres
{
    /// <summary>
    /// This is the storage repository for the TestDataTypesPostgres module.
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    public partial class TestDataTypesStorageRepository<TDomain> : EntityFrameworkCoreStorageRepository<TDomain>
        where TDomain : class, IEntityFrameworkCoreDomainObject<TDomain>, new()
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logFactory"></param>
        /// <param name="context"></param>
        public TestDataTypesStorageRepository(
            ILoggerFactory logFactory,
            TestDataTypesPostgresContext context)
            : base(logFactory)
        {
            Context = context;
            DbSet = context.Set<TDomain>();
        }
    }
}
