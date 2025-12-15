using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBricks.TestDataTypes.EntityFrameworkCore;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.TestDataTypes.Postgres
{
    /// <summary>
    /// This rule is executed when the TestDataTypesPostgres module is added.
    /// </summary>
    public sealed class TestDataTypesPostgresModuleAddRule : BusinessRule
    {
        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleAddEvent<TestDataTypesPostgresModule>),
                typeof(TestDataTypesPostgresModuleAddRule));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleAddEvent<TestDataTypesPostgresModule>),
                typeof(TestDataTypesPostgresModuleAddRule));
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            // AI: Make sure the context object is the correct type
            if (context == null || context.Object == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }
            var e = context.Object as ModuleAddEvent<TestDataTypesPostgresModule>;
            if (e == null || e.DomainObject == null || e.ServiceCollection == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            var services = e.ServiceCollection;
            var configuration = e.Configuration;

            // AI: Register the database for the module
            var builder = new DbContextOptionsBuilder<TestDataTypesPostgresContext>();
            string connectionString = configuration.GetPostgresConnectionString(
                TestDataTypesPostgresConstants.APPSETTING_CONNECTION_STRING);
            builder.UseNpgsql(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.GetName().Name);
                x.EnableRetryOnFailure();
            });
            services.Configure<DbContextOptions<TestDataTypesPostgresContext>>(o => { o = builder.Options; });
            services.AddSingleton<DbContextOptions<TestDataTypesPostgresContext>>(builder.Options);
            services.AddDbContext<TestDataTypesPostgresContext>(c => { c = builder; }, ServiceLifetime.Scoped);

            // AI: Add the storage services for the module for each domain object
            
            services.AddScoped<IStorageRepository<Test>, TestDataTypesStorageRepository<Test>>();


            // AI: Register business rules for the module
         TestCharTranslationCreateUpdateRule.Register(BusinessRuleRegistry.Instance);


            return response;
        }
    }
}
