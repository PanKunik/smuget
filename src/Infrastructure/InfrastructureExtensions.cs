using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistance;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext();

        services.AddSingleton<ExceptionMiddleware>();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}