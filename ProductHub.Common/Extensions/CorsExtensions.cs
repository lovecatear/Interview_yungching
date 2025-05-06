using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProductHub.Common.Extensions;

/// <summary>
/// CORS configuration for handling cross-origin requests
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Setup CORS policy with configurable origins
    /// </summary>
    /// <param name="services">Service collection for DI</param>
    /// <param name="configuration">App settings</param>
    /// <param name="environment">Runtime environment</param>
    /// <returns></returns>
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                // Get origins from config
                var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

                if (allowedOrigins != null && allowedOrigins.Length != 0)
                {
                    // Use config origins
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                }
                else
                {
                    if (environment.IsDevelopment())
                    {
                        // Dev: allow all
                        policy.SetIsOriginAllowed(origin => true)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    }
                }
            });
        });

        return services;
    }
}