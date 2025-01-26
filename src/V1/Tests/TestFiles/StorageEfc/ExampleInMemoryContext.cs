using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ServiceBricks.Storage.EntityFrameworkCore.Xunit
{
    /// <summary>
    /// The database context for Logging.
    /// </summary>
    public partial class ExampleInMemoryContext : DbContext
    {
        protected DbContextOptions<ExampleInMemoryContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExampleInMemoryContext() : base()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<ExampleInMemoryContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _options = builder.Options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public ExampleInMemoryContext(DbContextOptions<ExampleInMemoryContext> options) : base(options)
        {
            _options = options;
        }

        public virtual DbSet<ExampleDomain> Examples { get; set; }

        public virtual DbSet<ExampleWorkProcessDomain> ExampleProcessQueue { get; set; }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ExampleDomain>().HasKey(key => key.Key);
            builder.Entity<ExampleWorkProcessDomain>().HasKey(key => key.Key);
        }
    }
}