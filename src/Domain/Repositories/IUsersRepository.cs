using Domain.Users;

namespace Domain.Repositories;

public interface IUsersRepository
{
    Task<User?> GetByEmail(Email email);
    Task Save(User user);
}