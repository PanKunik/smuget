using Domain.Exceptions;

namespace Domain.RefreshTokens;

public sealed class RefreshToken
{
    public RefreshTokenId Id { get; }
    public string Token { get; }
    public DateTime Expires { get; }
    public bool WasUsed { get; }

    public RefreshToken(
        RefreshTokenId refreshTokenId,
        string token,
        DateTime expires,
        bool wasUsed
    )
    {
        ThrowIfRefreshTokenIdIsNull(refreshTokenId);
        ThrowIfTokenIsNullOrWhiteSpace(token);

        Id = refreshTokenId;
        Token = token;
        Expires = expires;
        WasUsed = wasUsed;
    }

    private void ThrowIfRefreshTokenIdIsNull(RefreshTokenId refreshTokenId)
    {
        if (refreshTokenId is null)
        {
            throw new RefreshTokenIdIsNullException();
        }
    }

    private void ThrowIfTokenIsNullOrWhiteSpace(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidTokenException();
        }
    }
}