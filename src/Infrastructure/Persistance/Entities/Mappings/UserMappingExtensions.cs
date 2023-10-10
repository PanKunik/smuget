using Domain.Users;

namespace Infrastructure.Persistance.Entities.Mappings;

internal static class UserMappingExtensions
{
    public static User ToDomain(this UserEntity entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new User(
            new UserId(entity.Id),
            new Email(entity.Email),
            new FirstName(entity.FirstName),
            entity.SecuredPassword
        );
    }

    public static UserEntity ToEntity(this User domain)
    {
        return new UserEntity()
        {
            Id = domain.Id.Value,
            Email = domain.Email.Value,
            FirstName = domain.FirstName.Value,
            SecuredPassword = domain.SecuredPassword
        };
    }
}