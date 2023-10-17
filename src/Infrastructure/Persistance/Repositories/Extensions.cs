using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance.Repositories;

internal static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IMonthlyBillingsRepository, MonthlyBillingsRepository>();
        services.AddTransient<IUsersRepository, UsersRepository>();
        services.AddTransient<IRefreshTokensRepository, RefreshTokensRepository>();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        return services;
    }
}