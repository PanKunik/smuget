using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class RefreshTokenNotFoundException : SmugetException
{
    public RefreshTokenNotFoundException()
        : base("Refresh token not found.") { }
}