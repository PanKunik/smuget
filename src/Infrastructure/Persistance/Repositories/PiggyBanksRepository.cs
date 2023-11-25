using Domain.MonthlyBillings;
using Domain.PiggyBanks;
using Domain.Repositories;
using Infrastructure.Persistance.Entities.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

internal sealed class PiggyBanksRepository
    : IPiggyBanksRepository
{
    private readonly SmugetDbContext _dbContext;

    public PiggyBanksRepository(SmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PiggyBank?> GetByName(
        Name name,
        CancellationToken token = default
    )
    {
        var entity = await _dbContext.PiggyBanks
            .Include(p => p.Transactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Name == name.Value
            );

        return entity?.ToDomain();
    }

    public async Task Save(PiggyBank piggyBank)
    {
        var newEntity = piggyBank.ToEntity();
        var existingEntity = await _dbContext.PiggyBanks
            .Include(p => p.Transactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Id == piggyBank.Id.Value
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
