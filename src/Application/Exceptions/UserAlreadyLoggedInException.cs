namespace Application.Exceptions;

public sealed class UserAlreadyLoggedInException
    : ForbiddenException
{
    public UserAlreadyLoggedInException()
        : base("User is already logged in!") { }
}