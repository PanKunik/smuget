namespace Application.Exceptions;

public sealed class RefreshTokenUsedException
    : ForbiddenException
{
    public RefreshTokenUsedException()
        : base("Refresh token was used before.") { }
}