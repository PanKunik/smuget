namespace WebAPI.Middlewares;

public static class HTTPLoggingExtensions
{
    public static IServiceCollection AddHTTPLogging(this IServiceCollection services)
    {
        services.AddSingleton<HTTPLoggingMiddleware>();

        return services;
    }

    public static WebApplication UseHTTPLogging(this WebApplication builder)
    {
        builder.UseMiddleware<HTTPLoggingMiddleware>();

        return builder;
    }
}