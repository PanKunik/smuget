using Domain.RefreshTokens;

namespace Infrastructure.Persistance.Entities.Mappings;

internal static class RefreshTokenMappingExtensions
{
    public static RefreshToken ToDomain(this RefreshTokenEntity entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new RefreshToken(
            new(entity.Id),
            entity.Token,
            entity.JwtId,
            entity.CreationDateTime,
            entity.ExpirationDateTime,
            entity.Used,
            entity.Invalidated,
            new(entity.UserId)
        );
    }

    public static RefreshTokenEntity ToEntity(this RefreshToken domain)
    {
        return new RefreshTokenEntity()
        {
            Id = domain.Id.Value,
            Token = domain.Token,
            JwtId = domain.JwtId,
            CreationDateTime = domain.CreationDateTime,
            ExpirationDateTime = domain.ExpirationDateTime,
            Used = domain.Used,
            Invalidated = domain.Invalidated,
            UserId = domain.UserId.Value
        };
    }
}