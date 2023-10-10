using Application.Abstractions.Security;
using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security;

internal sealed class PasswordHasher : IPasswordHasher
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordHasher(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string Secure(string password)
        => _passwordHasher.HashPassword(
                default,
                password
            );
}
