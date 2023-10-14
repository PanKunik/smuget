using Domain.Exceptions;
using Domain.Users;

namespace Domain.RefreshTokens;

public sealed class RefreshToken
{
    public RefreshTokenId Id { get; }
    public string Token { get; }
    public DateTime Expires { get; }
    public bool WasUsed { get; }
    public UserId UserId { get; }

    public RefreshToken(
        RefreshTokenId refreshTokenId,
        string token,
        DateTime expires,
        bool wasUsed,
        UserId userId
    )
    {
        ThrowIfRefreshTokenIdIsNull(refreshTokenId);
        ThrowIfTokenIsNullOrWhiteSpace(token);
        ThrowIfUserIdIsNull(userId);

        Id = refreshTokenId;
        Token = token;
        Expires = expires;
        WasUsed = wasUsed;
        UserId = userId;
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

    private void ThrowIfUserIdIsNull(UserId userId)
    {
        if (userId is null)
        {
            throw new UserIdIsNullException();
        }
    }
}