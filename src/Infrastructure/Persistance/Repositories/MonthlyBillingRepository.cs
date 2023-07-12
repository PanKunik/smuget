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
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(newEntity);
            await SaveIncomes(existingEntity, newEntity.Incomes);
            await SavePlans(existingEntity, newEntity.Plans);
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task SaveIncomes(MonthlyBillingEntity existingEntity, List<IncomeEntity> incomeEntities)
    {
        foreach (var incomeEntity in incomeEntities)
        {
            var existingIncomeEntity = existingEntity.Incomes.Find(i => i.Id == incomeEntity.Id);
            if (existingIncomeEntity is not null)
            {
                _dbContext.Entry(existingIncomeEntity).CurrentValues.SetValues(incomeEntity);
            }
            else
            {
                await _dbContext.AddAsync(incomeEntity);
            }
        }
    }

    private async Task SavePlans(MonthlyBillingEntity existingEntity, List<PlanEntity> planEntities)
    {
        foreach (var planEntity in planEntities)
        {
            var existingPlanEntity = existingEntity.Plans.Find(p => p.Id == planEntity.Id);
            if (existingPlanEntity is not null)
            {
                _dbContext.Entry(existingPlanEntity).CurrentValues.SetValues(planEntity);
            }
            else
            {
                var result = await _dbContext.AddAsync(planEntity);
                existingPlanEntity = result.Entity;
            }

            await SaveExpenses(existingPlanEntity, planEntity.Expenses);
        }
    }

    private async Task SaveExpenses(PlanEntity existingEntity, List<ExpenseEntity> expenseEntities)
    {
        foreach (var expenseEntity in expenseEntities)
        {
            var existingExpenseEntity = existingEntity.Expenses.Find(e => e.Id == expenseEntity.Id);
            if (existingExpenseEntity is not null)
            {
                _dbContext.Entry(existingExpenseEntity).CurrentValues.SetValues(expenseEntity);
            }
            else
            {
                await _dbContext.AddAsync(expenseEntity);
            }
        }
    }
}