using Microsoft.AspNetCore.CookiePolicy;

namespace WebApp.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }

        public static IApplicationBuilder StartCustomWebsite(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[] { "en-US", "es" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
            app.UseRequestLocalization(localizationOptions);

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

            app.UseCookiePolicy();
            app.UseMiddleware<CookiePolicyMiddleware>();

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

            return app;
        }
    }
}