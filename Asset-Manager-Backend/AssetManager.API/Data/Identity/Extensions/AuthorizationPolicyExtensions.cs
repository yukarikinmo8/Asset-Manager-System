using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace AssetManager.API.Data.Identity.Extensions;

public static class AuthorizationPolicyExtensions
{
    public static IServiceCollection AddCustomPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "EmployeeOnly",
                policy =>
                    policy
                        .RequireRole("Employee")
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            );

            options.AddPolicy(
                "AssetManagerOnly",
                policy =>
                    policy
                        .RequireRole("AssetManager")
                        // .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme) // Use IdentityConstants for cookie authentication
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme) // Use JwtBearerDefaults for JWT authentication
            );
        });

        return services;
    }
}
