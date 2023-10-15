using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class RefreshTokenUsedException : SmugetException
{
    public RefreshTokenUsedException()
        : base("Refresh token was used before.") { }
}