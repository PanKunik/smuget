using Application.Identity;
using Domain.Users;

namespace Application.Abstractions.Authentication;

public interface IAuthenticator
{
    AuthenticationDTO CreateToken(User user);
    AuthenticationDTO RefreshToken(
        User user,
        string accessToken,
        Guid refreshTokenJwtId
    );
}