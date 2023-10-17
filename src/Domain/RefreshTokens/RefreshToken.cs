using Domain.Exceptions;
using Domain.Users;

namespace Domain.RefreshTokens;

public sealed class RefreshToken
{
    public RefreshTokenId Id { get; }
    public string Token { get; }
    public Guid JwtId { get; }
    public DateTime CreationDateTime { get; }
    public DateTime ExpirationDateTime { get; }
    public bool Used { get; private set; }
    public bool Invalidated { get; private set; }
    public UserId UserId { get; }

    public RefreshToken(
        RefreshTokenId refreshTokenId,
        string token,
        Guid jwtId,
        DateTime creationDateTime,
        DateTime expirationDateTime,
        bool used,
        bool invalidated,
        UserId userId
    )
    {
        ThrowIfRefreshTokenIdIsNull(refreshTokenId);
        ThrowIfTokenIsNullOrWhiteSpace(token);
        ThrowIfUserIdIsNull(userId);

        Id = refreshTokenId;
        Token = token;
        JwtId = jwtId;
        CreationDateTime = creationDateTime;
        ExpirationDateTime = expirationDateTime;
        Used = used;
        Invalidated = invalidated;
        UserId = userId;
    }

    public void Use()
        => Used = true;

    public void Invalidate()
        => Invalidated = true;

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