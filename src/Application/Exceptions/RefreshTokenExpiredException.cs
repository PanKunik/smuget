using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class RefreshTokenExpiredException : SmugetException
{
    public RefreshTokenExpiredException()
        : base("Refresh token has expired.") { }
}