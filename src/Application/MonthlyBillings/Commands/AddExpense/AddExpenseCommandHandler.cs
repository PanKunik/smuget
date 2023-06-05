using Application.Abstractions.CQRS;
using Application.Abstractions.Persistance;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;

namespace Application.MonthlyBillings.Commands.AddExpense;

public sealed class AddExpenseCommandHandler : ICommandHandler<AddExpenseCommand>
{
    private readonly ISmugetDbContext _dbContext;

    public AddExpenseCommandHandler(ISmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(AddExpenseCommand command, CancellationToken cancellationToken = default)
    {
        var monthlyBilling = await _dbContext.MonthlyBillings
            .Include(i => i.Incomes)
            .Include(i => i.Plans)
            .ThenInclude(p => p.Expenses)
            .FirstOrDefaultAsync(
                m => m.Id.Value == command.MonthlyBillingId,
                cancellationToken
            );

        if (monthlyBilling is null)
        {
            throw new MonthlyBillingNotFoundException();
        }

        var expense = new Expense(
            new Money(command.Money, command.Currency),
            command.ExpenseDate,
            command.Description
        );

        monthlyBilling.AddExpense(
            new PlanId(command.PlanId),
            expense);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}