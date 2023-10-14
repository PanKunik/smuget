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
            entity.Expires,
            entity.WasUsed,
            new(entity.UserId)
        );
    }

    public static RefreshTokenEntity ToEntity(this RefreshToken domain)
    {
        return new RefreshTokenEntity()
        {
            Id = domain.Id.Value,
            Token = domain.Token,
            Expires = domain.Expires,
            WasUsed = domain.WasUsed,
            UserId = domain.UserId.Value
        };
    }
}