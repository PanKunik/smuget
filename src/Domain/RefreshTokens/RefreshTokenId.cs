using Domain.Exceptions;

namespace Domain.RefreshTokens;

public sealed record RefreshTokenId
{
    public Guid Value { get; }

    public RefreshTokenId(
        Guid value
    )
    {
        ThrowIfValueIsEqualToEmptyGuid(value);
        Value = value;
    }

    private void ThrowIfValueIsEqualToEmptyGuid(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidRefreshTokenIdException();
        }
    }
}