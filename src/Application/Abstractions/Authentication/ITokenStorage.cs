using Application.Identity;

namespace Application.Abstractions.Authentication;

public interface ITokenStorage
{
    void Store(AuthenticationDTO token);
    AuthenticationDTO Get();
}