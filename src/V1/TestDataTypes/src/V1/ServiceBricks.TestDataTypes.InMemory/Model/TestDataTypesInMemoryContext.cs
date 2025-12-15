using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.InMemory
{
    /// <summary>
    /// The database context for the TestDataTypesInMemory module.
    /// </summary>
    public partial class TestDataTypesInMemoryContext : DbContext
    {
        protected DbContextOptions<TestDataTypesInMemoryContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestDataTypesInMemoryContext() : base()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<TestDataTypesInMemoryContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _options = builder.Options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public TestDataTypesInMemoryContext(DbContextOptions<TestDataTypesInMemoryContext> options) : base(options)
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

        }

        /// <summary>
        /// Create context.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual TestDataTypesInMemoryContext CreateDbContext(string[] args)
        {
            return new TestDataTypesInMemoryContext(_options);
        }
    }
}
