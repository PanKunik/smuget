using Application.Abstractions.Security;
using Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Application.Abstractions.Authentication;
using Infrastructure.Authentication;

namespace Infrastructure.Security;

internal static class SecurityExtensions
{
    internal static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthenticator, Authenticator>();
        services.AddSingleton<ITokenStorage, HttpContextTokenStorage>();

        return services;
    }
}