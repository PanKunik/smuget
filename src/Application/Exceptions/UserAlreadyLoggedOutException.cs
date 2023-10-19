using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class UserAlreadyLoggedOutException : SmugetException
{
    public UserAlreadyLoggedOutException()
        : base("User is already logged out!") { }
}