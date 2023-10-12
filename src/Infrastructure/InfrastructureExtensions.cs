using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistance;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;
using Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.Configuration;
using Infrastructure.Security;
using Infrastructure.Authentication;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext(configuration);
        services.AddRepositories();        
        services.AddHttpContextAccessor();
        services.AddAuthenticationWithJwt(configuration);
        services.AddSecurity();

        services.AddSingleton<ExceptionMiddleware>();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }

    public static T GetOptions<T>(
        this IConfiguration configuration,
        string sectionName
    ) where T : class, new()
    {
        var options = new T();
        configuration
            .GetRequiredSection(sectionName)
            .Bind(options);

        return options;
    }
}