using ServiceBricks;

//using ServiceBricks.Logging;
//using ServiceBricks.Security;

namespace WebApp.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<TrapExceptionResponseMiddleware>();

            //app.UseMiddleware<LogMessageMiddleware>();
            //app.UseMiddleware<WebRequestMessageMiddleware>();
            //app.UseMiddleware<PropogateExceptionResponseMiddleware>();
            return app;
        }

        public static IApplicationBuilder StartCustomWebsite(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            // Register Middleware after UseAuth() so user context is available
            app.RegisterMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    x.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
                });
            }

            // Create a default admin test user account
            app.CreateTestUserAccount();

            return app;
        }

        private static IApplicationBuilder CreateTestUserAccount(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //// Create process to register a new admin user
                //UserRegisterAdminProcess registerAdminProcess =
                //    new UserRegisterAdminProcess(
                //        "unittest@servicebricks.com",
                //        "UnitTest123!@#");

                //// Get Business Rule Service
                //var businessRuleService = scope.ServiceProvider.GetRequiredService<IBusinessRuleService>();

                //// Execute the process
                //var response = businessRuleService.ExecuteProcess(registerAdminProcess);
            }

            return builder;
        }
    }
}