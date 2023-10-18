namespace Infrastructure.Exceptions;

public sealed class InvalidRefreshTokenException : IdentityException
{
    public InvalidRefreshTokenException()
        : base("Invalid refresh token.") { }
}