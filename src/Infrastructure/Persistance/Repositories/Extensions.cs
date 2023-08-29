using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance.Repositories;

internal static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IMonthlyBillingRepository, MonthlyBillingRepository>();

        return services;
    }
}