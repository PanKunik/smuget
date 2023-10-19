using Application.Abstractions.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommandsAndQueries();
        return services;
    }
}