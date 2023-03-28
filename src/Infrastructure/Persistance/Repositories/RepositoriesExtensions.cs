using Application.Abstractions.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance.Repositories;

internal static class RepositoriesExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMonthlyBillingsRepository, MonthlyBillingsRepository>();

        return services;
    }
}