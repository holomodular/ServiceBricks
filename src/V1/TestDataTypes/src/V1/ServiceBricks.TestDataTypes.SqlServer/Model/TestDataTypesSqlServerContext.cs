using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using ServiceBricks.Storage.EntityFrameworkCore;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

// dotnet ef migrations add TestDataTypesV1 --context TestDataTypesSqlServerContext --startup-project ../Tests/MigrationsHost

namespace ServiceBricks.TestDataTypes.SqlServer
{
    /// <summary>
    /// The database context for the TestDataTypesSqlServer module.
    /// </summary>
    public partial class TestDataTypesSqlServerContext : DbContext
    {
        protected DbContextOptions<TestDataTypesSqlServerContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestDataTypesSqlServerContext() : base()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<TestDataTypesSqlServerContext>();
            string connectionString = configuration.GetSqlServerConnectionString(
                TestDataTypesSqlServerConstants.APPSETTING_CONNECTION_STRING);
            builder.UseSqlServer(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(TestDataTypesSqlServerContext).Assembly.GetName().Name);
                x.EnableRetryOnFailure();
            });
            _options = builder.Options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public TestDataTypesSqlServerContext(DbContextOptions<TestDataTypesSqlServerContext> options) : base(options)
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
            builder.HasDefaultSchema(TestDataTypesSqlServerConstants.DATABASE_SCHEMA_NAME);

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
        public virtual TestDataTypesSqlServerContext CreateDbContext(string[] args)
        {
            return new TestDataTypesSqlServerContext(_options);
        }
    }
}
