namespace Application.Exceptions;

public sealed class InvalidRefreshTokenException
    : IdentityException
{
    public InvalidRefreshTokenException(string message)
        : base(message) { }
}