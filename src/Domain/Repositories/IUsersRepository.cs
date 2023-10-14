using Domain.Users;

namespace Domain.Repositories;

public interface IUsersRepository
{
    Task<User?> GetByEmail(Email email);
    Task<User?> Get(UserId userId);
    Task Save(User user);
}