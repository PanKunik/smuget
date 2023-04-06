using Microsoft.Extensions.DependencyInjection;

namespace Application.Abstractions.CQRS;

internal static class CQRSExtensions
{
    internal static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
    {
        services.Scan(a => a.FromAssemblies(typeof(ICommandHandler<>).Assembly)
                            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                            .AsImplementedInterfaces()
                            .WithScopedLifetime());

        return services;
    }
}