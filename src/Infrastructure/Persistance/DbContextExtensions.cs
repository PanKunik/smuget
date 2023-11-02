using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance;

internal static class DbContextExtensions
{
    internal static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<SmugetDbContext>(
            c => c.UseNpgsql(configuration.GetConnectionString("smuget-db"))
        );

        services.AddHostedService<DatabaseInitializer>();

        return services;
    }
}