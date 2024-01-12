using Infrastructure.Exceptions;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace WebAPI.Extensions;

public static class AddRateLimitingExtension
{
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
        => app.UseRateLimiter(new RateLimiterOptions
        {
            GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
            context =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    context.Request.Headers["X-Forwarded-For"].ToString(),
                    httpContext => new FixedWindowRateLimiterOptions()
                    {
                        PermitLimit = 2,
                        QueueLimit = 0,
                        AutoReplenishment = true,
                        Window = TimeSpan.FromSeconds(1)
                    });
            }),

            OnRejected = async (context, h) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsJsonAsync(
                    new Error(
                        "Too many requests. There is a limit of 2 requests per second.",
                        "TooManyRequests",
                        context.HttpContext.Request.Path
                    )
                );
            }
        });
}
