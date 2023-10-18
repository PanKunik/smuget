using Domain.RefreshTokens;
using Domain.Repositories;
using Infrastructure.Persistance.Entities.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

internal sealed class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly SmugetDbContext _dbContext;

    public RefreshTokensRepository(SmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshToken?> Get(string token)
    {
        var entity = await _dbContext.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Token == token
            );

        return entity?
            .ToDomain();
    }

    public async Task<RefreshToken?> GetByJwtId(Guid jwtId)
    {
        var entity = await _dbContext.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.JwtId == jwtId
            );

        return entity?
            .ToDomain();
    }

    public async Task Save(RefreshToken refreshToken)
    {
        var newEntity = refreshToken.ToEntity();
        var existingEntity = await _dbContext.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Id == refreshToken.Id.Value
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