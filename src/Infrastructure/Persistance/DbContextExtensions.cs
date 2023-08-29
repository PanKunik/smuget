using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance;

internal static class DbContextExtensions
{
    internal static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<SmugetDbContext>(
            c => c.UseNpgsql("Host=db;Database=postgres;Username=postgres;Password=example")
        );

        services.AddHostedService<DatabaseInitializer>();

        return services;
    }
}