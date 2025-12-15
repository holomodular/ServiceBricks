using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using ServiceBricks.Storage.EntityFrameworkCore;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;

// dotnet ef migrations add TestDataTypesV1 --context TestDataTypesSqliteContext --startup-project ../Tests/MigrationsHost

namespace ServiceBricks.TestDataTypes.Sqlite
{
    /// <summary>
    /// The database context for the TestDataTypesSqlite module.
    /// </summary>
    public partial class TestDataTypesSqliteContext : DbContext
    {
        protected DbContextOptions<TestDataTypesSqliteContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestDataTypesSqliteContext() : base()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<TestDataTypesSqliteContext>();
            string connectionString = configuration.GetSqliteConnectionString(
                TestDataTypesSqliteConstants.APPSETTING_CONNECTION_STRING);
            builder.UseSqlite(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(TestDataTypesSqliteContext).Assembly.GetName().Name);
            });
            _options = builder.Options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public TestDataTypesSqliteContext(DbContextOptions<TestDataTypesSqliteContext> options) : base(options)
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
                .Properties<DateTimeOffset>()
                .HaveConversion<DateTimeOffsetToBytesConverter>();

            base.ConfigureConventions(configurationBuilder);
        }

        /// <summary>
        /// Create context.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual TestDataTypesSqliteContext CreateDbContext(string[] args)
        {
            return new TestDataTypesSqliteContext(_options);
        }
    }
}
