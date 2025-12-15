using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using ServiceBricks.Storage.EntityFrameworkCore;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

// dotnet ef migrations add TestDataTypesV1 --context TestDataTypesPostgresContext --startup-project ../Tests/MigrationsHost

namespace ServiceBricks.TestDataTypes.Postgres
{
    /// <summary>
    /// The database context for the TestDataTypesPostgres module.
    /// </summary>
    public partial class TestDataTypesPostgresContext : DbContext
    {
        protected DbContextOptions<TestDataTypesPostgresContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestDataTypesPostgresContext() : base()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<TestDataTypesPostgresContext>();
            string connectionString = configuration.GetPostgresConnectionString(
                TestDataTypesPostgresConstants.APPSETTING_CONNECTION_STRING);
            builder.UseNpgsql(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(TestDataTypesPostgresContext).Assembly.GetName().Name);
                x.EnableRetryOnFailure();
            });
            _options = builder.Options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public TestDataTypesPostgresContext(DbContextOptions<TestDataTypesPostgresContext> options) : base(options)
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
            // AI: Set the default schema
            builder.HasDefaultSchema(TestDataTypesPostgresConstants.DATABASE_SCHEMA_NAME);

            // AI: Setup the entities to the model
            
            builder.Entity<Test>().HasKey(key => key.Key);


            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Configure conventions
        /// </summary>
        /// <param name="configurationBuilder"></param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<TimeSpan>()
                .HaveConversion<long>();

            base.ConfigureConventions(configurationBuilder);
        }

        /// <summary>
        /// Create context.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual TestDataTypesPostgresContext CreateDbContext(string[] args)
        {
            return new TestDataTypesPostgresContext(_options);
        }
    }
}
