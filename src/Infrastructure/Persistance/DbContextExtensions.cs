using Application.Abstractions.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance;

internal static class DbContextExtensions
{
    internal static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddScoped<ISmugetDbContext, SmugetDbContext>();
        services.AddDbContext<SmugetDbContext>(
            c => c.UseInMemoryDatabase("Smuget")
        );

        return services;
    }
}