using Domain.Repositories;
using Domain.Users;
using Infrastructure.Persistance.Entities.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

internal sealed class UsersRepository
    : IUsersRepository
{
    private readonly SmugetDbContext _dbContext;

    public UsersRepository(SmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmail(Email email)
    {
        var entity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Email == email.Value
            );

        return entity?
            .ToDomain();
    }

    public async Task<User?> Get(UserId userId)
    {
        var entiy = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Id == userId.Value
            );

        return entiy?
            .ToDomain();
    }

    public async Task Save(User user)
    {
        var newEntity = user.ToEntity();
        var existingEntity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Id == user.Id.Value
            );

        if (existingEntity is null)
        {
            await _dbContext.AddAsync(newEntity);
        }
        else
        {
            _dbContext.Update(newEntity);
        }

        await _dbContext.SaveChangesAsync();
    }
}