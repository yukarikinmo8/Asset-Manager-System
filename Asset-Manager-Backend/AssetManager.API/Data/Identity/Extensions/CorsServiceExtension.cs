using Microsoft.AspNetCore.Cors.Infrastructure;

namespace AssetManager.API.Data.Identity.Extensions;

public static class CorsServiceExtension
{
    /// <summary>
    /// Adds CORS (Cross-Origin Resource Sharing) services to the DI container
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to</param>
    /// <param name="configuration">The configuration instance</param>
    /// <returns>The IServiceCollection for method chaining</returns>
    public static IServiceCollection AddCorsService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                "MyCorsPolicy",
                policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            );
        });

        return services;
    }

    /// <summary>
    /// Adds a simple CORS policy with default settings
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to</param>
    /// <param name="policyName">The name of the CORS policy</param>
    /// <param name="allowedOrigins">Array of allowed origins</param>
    /// <returns>The IServiceCollection for method chaining</returns>
    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        string policyName,
        params string[] allowedOrigins
    )
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                policyName,
                policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            );
        });

        return services;
    }

    /// <summary>
    /// Adds a customizable CORS policy
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to</param>
    /// <param name="policyName">The name of the CORS policy</param>
    /// <param name="configurePolicy">Action to configure the CORS policy</param>
    /// <returns>The IServiceCollection for method chaining</returns>
    public static IServiceCollection AddCustomCorsPolicy(
        this IServiceCollection services,
        string policyName,
        Action<CorsPolicyBuilder> configurePolicy
    )
    {
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, configurePolicy);
        });

        return services;
    }
}
