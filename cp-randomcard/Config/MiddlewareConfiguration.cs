using Asp.Versioning.Conventions;
using cp_randomcard.RateLimit;
using cp_randomcard.RateLimit.Security;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System;

namespace cp_randomcard.Config
{
    public static class MiddlewareConfiguration
    {
        public static void ConfigureMiddlewares(this WebApplication app)
        {

            #region Rate Limiting Middleware
            app.UseMiddleware<RateLimitingMiddleware>();
            #endregion
            
            #region Authentication and Authorization Middleware
            app.UseAuthentication();
            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthorization();
            #endregion

            #region API Versioning
            var versionSet = app
                .NewApiVersionSet()
                .HasApiVersion(1.0)
                .ReportApiVersions()
                .Build();
            #endregion

            #region Security
            app.UseHttpsRedirection();
            #endregion

            #region Health Check Middleware
            app.MapHealthChecks("/api/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/health-ui";
            });
            #endregion

            #region Swagger and ReDoc Configuration
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var descriptions = app.DescribeApiVersions();

                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });

                foreach (var description in app.DescribeApiVersions())
                {
                    app.UseReDoc(options =>
                    {
                        options.DocumentTitle = $"API Documentation {description.GroupName}";
                        options.SpecUrl = $"/swagger/{description.GroupName}/swagger.json";
                        options.RoutePrefix = $"docs-{description.GroupName}";
                    });
                }
            }
            #endregion
        }
    }
}
