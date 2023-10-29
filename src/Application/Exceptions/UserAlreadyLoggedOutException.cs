namespace Application.Exceptions;

public sealed class UserAlreadyLoggedOutException
    : ForbiddenException
{
    public UserAlreadyLoggedOutException()
        : base("User is already logged out!") { }
}