using Domain.MonthlyBillings;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;
using Infrastructure.Persistance.Entities;
using Infrastructure.Persistance.Entities.Mappings;
using Microsoft.AspNetCore.Builder;
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
        UserId userId,
        CancellationToken token = default
    )
    {
        var entity = await _dbContext.PiggyBanks
            .Include(p => p.Transactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Name == name.Value
                  && p.UserId == userId.Value
            );

        return entity?.ToDomain();
    }

    public async Task<PiggyBank?> GetById(
        PiggyBankId piggyBankId,
        UserId userId,
        CancellationToken token = default
    )
    {
        var entity = await _dbContext.PiggyBanks
            .Include(p => p.Transactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Id == piggyBankId.Value
                  && p.UserId == userId.Value
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
                  && p.UserId == piggyBank.UserId.Value
            );

        if (existingEntity is null)
        {
            await _dbContext.AddAsync(newEntity);
        }
        else
        {
            _dbContext.Update(newEntity);
        }

        await SaveTransactions(
            existingEntity?.Transactions ?? new List<TransactionEntity>(),
            newEntity.Transactions
        );

        await _dbContext.SaveChangesAsync();
    }

    private async Task SaveTransactions(
        List<TransactionEntity> existingTransactions,
        List<TransactionEntity> transactionEntities
    )
    {
        foreach (var incomeEntity in transactionEntities)
        {
            if (existingTransactions.Any(i => i.Id == incomeEntity.Id))
            {
                _dbContext.Update(incomeEntity);
            }
            else
            {
                await _dbContext.AddAsync(incomeEntity);
            }
        }
    }
}
