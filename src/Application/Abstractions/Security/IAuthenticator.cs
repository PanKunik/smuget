using Application.Identity;
using Domain.Users;

namespace Application.Abstractions.Security;

public interface IAuthenticator
{
    AuthenticationDTO CreateToken(User user);
}