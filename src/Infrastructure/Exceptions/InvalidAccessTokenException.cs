namespace Infrastructure.Exceptions;

public sealed class InvalidAccessTokenException : IdentityException
{
    public InvalidAccessTokenException()
        : base("Invalid access token.") { }
}