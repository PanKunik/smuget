using Domain.Users;

namespace Application.Abstractions.Security;

public interface IPasswordHasher
{
    Task<string> Secure(Password password);
}