namespace Application.Exceptions;

public sealed class RefreshTokenExpiredException
    : IdentityException
{
    public RefreshTokenExpiredException()
        : base("Refresh token has expired.") { }
}