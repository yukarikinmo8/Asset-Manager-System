using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AssetManager.API.Data.Identity.Extensions;

public static class CustomHttpResponseExtensions
{
    // Extension method to add custom 403 Forbidden response middleware
    public static IApplicationBuilder UseCustomForbiddenResponse(this IApplicationBuilder app)
    {
        return app.Use(
            async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 403)
                {
                    context.Response.ContentType = "application/json";
                    var message = System.Text.Json.JsonSerializer.Serialize(
                        new
                        {
                            message = "Forbidden: You do not have permission to perform this action.",
                        }
                    );
                    await context.Response.WriteAsync(message);
                }
            }
        );
    }
}
