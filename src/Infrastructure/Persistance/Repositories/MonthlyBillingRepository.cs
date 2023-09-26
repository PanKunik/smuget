using Domain.MonthlyBillings;
using Domain.Repositories;
using Infrastructure.Persistance.Entities;
using Infrastructure.Persistance.Entities.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

internal sealed class MonthlyBillingRepository : IMonthlyBillingRepository
{
    private readonly SmugetDbContext _dbContext;

    public MonthlyBillingRepository(SmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MonthlyBilling?> Get(Year year, Month month)
    {
        var entites = await _dbContext.MonthlyBillings
            .Include(m => m.Incomes)
            .Include(m => m.Plans)
            .ThenInclude(p => p.Expenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                m => m.Year == year.Value
                && m.Month == month.Value
            );

        return entites?
            .ToDomain();
    }

    public async Task<MonthlyBilling?> GetById(MonthlyBillingId monthlyBillingId)
    {
        var entity = await _dbContext.MonthlyBillings
            .Include(m => m.Incomes)
            .Include(m => m.Plans)
            .ThenInclude(p => p.Expenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                m => m.Id == monthlyBillingId.Value
            );

        return entity?
            .ToDomain();
    }

    public async Task Save(MonthlyBilling monthlyBilling)
    {
        var newEntity = monthlyBilling.ToEntity();
        var existingEntity = await _dbContext.MonthlyBillings
            .Include(m => m.Incomes)
            .Include(m => m.Plans)
            .ThenInclude(p => p.Expenses)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                m => m.Id == monthlyBilling.Id.Value
            );

        if (existingEntity is null)
        {
            await _dbContext.AddAsync(newEntity);
        }
        else
        {
            _dbContext.Update(newEntity);
        }

        await SaveIncomes(
            existingEntity?.Incomes ?? new List<IncomeEntity>(),
            newEntity.Incomes
        );

        await SavePlans(
            existingEntity?.Plans ?? new List<PlanEntity>(),
            newEntity.Plans
        );

        await _dbContext.SaveChangesAsync();
    }

    private async Task SaveIncomes(List<IncomeEntity> existingIncomes, List<IncomeEntity> incomeEntities)
    {
        foreach (var incomeEntity in incomeEntities)
        {
            if (existingIncomes.Any(i => i.Id == incomeEntity.Id))
            {
                _dbContext.Update(incomeEntity);
            }
            else
            {
                await _dbContext.AddAsync(incomeEntity);
            }
        }
    }

    private async Task SavePlans(List<PlanEntity> existingPlans, List<PlanEntity> planEntities)
    {
        foreach (var planEntity in planEntities)
        {
            var existingPlan = existingPlans.FirstOrDefault(p => p.Id == planEntity.Id);

            if (existingPlan is null)
            {
                await _dbContext.AddAsync(planEntity);
            }
            else
            {
                _dbContext.Update(planEntity);
            }

            await SaveExpenses(
                existingPlan?.Expenses ?? new List<ExpenseEntity>(),
                planEntity.Expenses
            );
        }
    }

    private async Task SaveExpenses(List<ExpenseEntity> existingExpenses, List<ExpenseEntity> expenseEntities)
    {
        foreach (var expenseEntity in expenseEntities)
        {
            if (existingExpenses.Any(e => e.Id == expenseEntity.Id))
            {
                _dbContext.Update(expenseEntity);
            }
            else
            {
                await _dbContext.AddAsync(expenseEntity);
            }
        }
    }
}