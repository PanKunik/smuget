namespace Application.Exceptions;

public sealed class RefreshTokenNotFoundException
    : IdentityException
{
    public RefreshTokenNotFoundException()
        : base("Refresh token not found.") { }
}