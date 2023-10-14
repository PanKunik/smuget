using Application.Users;

namespace Application.Abstractions.Security;

public interface IAuthenticator
{
    AuthenticationDTO CreateToken(Guid userId);
}