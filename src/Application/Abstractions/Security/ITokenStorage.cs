using Application.Users;

namespace Application.Abstractions.Security;

public interface ITokenStorage
{
    void Store(AuthenticationDTO token);
    AuthenticationDTO Get();
}