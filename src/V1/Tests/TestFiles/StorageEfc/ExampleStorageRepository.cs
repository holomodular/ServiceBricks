using Microsoft.Extensions.Logging;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    public class ExampleStorageRepository<TDomain> : EntityFrameworkCoreStorageRepository<TDomain>
        where TDomain : class, IEntityFrameworkCoreDomainObject<TDomain>, new()
    {
        public ExampleStorageRepository(
            ILoggerFactory logFactory,
            ExampleInMemoryContext context)
            : base(logFactory)
        {
            Context = context;
            DbSet = context.Set<TDomain>();
        }
    }
}