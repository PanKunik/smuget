using Domain.RefreshTokens;

namespace Domain.Exceptions;

public sealed class InvalidRefreshTokenIdException
    : RequiredFieldException
{
    public InvalidRefreshTokenIdException()
        : base(nameof(RefreshTokenId)) { }
}
