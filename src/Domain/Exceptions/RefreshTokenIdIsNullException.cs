using Domain.RefreshTokens;

namespace Domain.Exceptions;

public sealed class RefreshTokenIdIsNullException
    : RequiredFieldException
{
    public RefreshTokenIdIsNullException()
        : base(nameof(RefreshTokenId)) { }
}