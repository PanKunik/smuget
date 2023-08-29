using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistance;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;
using Infrastructure.Persistance.Repositories;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext();
        services.AddRepositories();

        services.AddSingleton<ExceptionMiddleware>();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}