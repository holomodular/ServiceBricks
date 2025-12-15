using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ServiceBricks.TestDataTypes.Cosmos
{
    /// <summary>
    /// The database context for the TestDataTypesCosmos module.
    /// </summary>
    public partial class TestDataTypesCosmosContext : DbContext
    {
        protected DbContextOptions<TestDataTypesCosmosContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public TestDataTypesCosmosContext(DbContextOptions<TestDataTypesCosmosContext> options) : base(options)
        {
            _options = options;
        }

        
        /// <summary>
        /// This a test object.
        /// </summary>
        public virtual DbSet<Test> Test { get; set; }


        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AI: Setup the entities to the model
            
        builder.Entity<Test>().HasKey(key => key.Key);
        builder.Entity<Test>().ToContainer(TestDataTypesCosmosConstants.GetContainerName(nameof(Test)));



        }



        /// <summary>
        /// OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if NET8_0_OR_GREATER
            optionsBuilder.ConfigureWarnings(w => w.Ignore(CosmosEventId.SyncNotSupported));
#endif

            base.OnConfiguring(optionsBuilder);
        }


        /// <summary>
        /// Create context.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual TestDataTypesCosmosContext CreateDbContext(string[] args)
        {
            return new TestDataTypesCosmosContext(_options);
        }

        /// <summary>
        /// Configure conventions
        /// </summary>
        /// <param name="configurationBuilder"></param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
        }
    }
}
