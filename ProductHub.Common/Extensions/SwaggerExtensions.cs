using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ProductHub.Common.Extensions;

/// <summary>
/// Extension methods for configuring Swagger documentation in the application.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Adds Swagger documentation services to the application's service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ProductHub API",
                Version = "v1",
                Description = "ProductHub API Documentation. For any questions or issues, please contact us at support@producthub.com",
                Contact = new OpenApiContact
                {
                    Name = "ProductHub Support",
                    Email = "support@producthub.com"
                }
            });

            // Configure XML documentation path
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly is not null)
            {
                var xmlFile = $"{entryAssembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            }
        });

        return services;
    }

    /// <summary>
    /// Configures the application to use Swagger and SwaggerUI middleware.
    /// </summary>
    /// <param name="app">The application builder instance.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductHub API V1");
            c.RoutePrefix = "swagger";
            c.DocumentTitle = "ProductHub API Documentation";
        });

        return app;
    }
}