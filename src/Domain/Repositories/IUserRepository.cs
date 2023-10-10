using Domain.Users;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmail(Email email);
    Task Save(User user);
}