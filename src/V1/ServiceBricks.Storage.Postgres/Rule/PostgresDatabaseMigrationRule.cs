﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks.Storage.Postgres
{
    /// <summary>
    /// This rule migrates a database.
    /// </summary>
    public sealed class PostgresDatabaseMigrationRule<TModule, TDatabaseContext> : BusinessRule
        where TModule : IModule
        where TDatabaseContext : DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PostgresDatabaseMigrationRule()
        {
            Priority = PRIORITY_HIGH;
        }

        /// <summary>
        /// Register the rule
        /// </summary>
        public static void Register(IBusinessRuleRegistry registry)
        {
            registry.Register(
                typeof(ModuleStartEvent<TModule>),
                typeof(PostgresDatabaseMigrationRule<TModule, TDatabaseContext>));
        }

        /// <summary>
        /// UnRegister the rule.
        /// </summary>
        public static void UnRegister(IBusinessRuleRegistry registry)
        {
            registry.UnRegister(
                typeof(ModuleStartEvent<TModule>),
                typeof(PostgresDatabaseMigrationRule<TModule, TDatabaseContext>));
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
            var e = context.Object as ModuleStartEvent<TModule>;
            if (e == null || e.DomainObject == null || e.ApplicationBuilder == null)
            {
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.PARAMETER_MISSING, "context"));
                return response;
            }

            // AI: Perform logic
            using (var serviceScope = e.ApplicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var databaseContext = serviceScope.ServiceProvider.GetService<TDatabaseContext>();
                databaseContext.Database.Migrate();
                databaseContext.SaveChanges();
            }

            return response;
        }
    }
}