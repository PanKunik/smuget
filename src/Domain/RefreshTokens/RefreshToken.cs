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
    public string IssuedFrom { get; set; }
    public bool Used { get; private set; }
    public bool Invalidated { get; private set; }
    public UserId UserId { get; }
    public RefreshTokenId? RefreshedBy { get; private set; }

    public RefreshToken(
        RefreshTokenId refreshTokenId,
        string token,
        Guid jwtId,
        DateTime creationDateTime,
        DateTime expirationDateTime,
        string ipAddress,
        bool used,
        bool invalidated,
        UserId userId,
        RefreshTokenId? refreshedBy = null
    )
    {
        ThrowIfRefreshTokenIdIsNull(refreshTokenId);
        ThrowIfTokenIsNullOrWhiteSpace(token);
        ThrowIfUserIdIsNull(userId);
        ThrowIfIpAddressIsNullOrWhiteSpace(ipAddress);

        Id = refreshTokenId;
        Token = token;
        JwtId = jwtId;
        CreationDateTime = creationDateTime;
        ExpirationDateTime = expirationDateTime;
        IssuedFrom = ipAddress;
        Used = used;
        Invalidated = invalidated;
        UserId = userId;
        RefreshedBy = refreshedBy;
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

    private void ThrowIfIpAddressIsNullOrWhiteSpace(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            throw new InvalidIpAddressException();
        }
    }
}