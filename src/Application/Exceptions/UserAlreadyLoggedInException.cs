using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class UserAlreadyLoggedInException : SmugetException
{
    public UserAlreadyLoggedInException()
        : base("User is already logged in!") { }
}