using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistance;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext();

        return services;
    }
}