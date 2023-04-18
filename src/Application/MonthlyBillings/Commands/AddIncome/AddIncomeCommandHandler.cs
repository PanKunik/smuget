using Application.Abstractions.CQRS;
using Application.Abstractions.Persistance;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;

namespace Application.MonthlyBillings.Commands.AddIncome;

public sealed class AddIncomeCommandHandler : ICommandHandler<AddIncomeCommand>
{
    private readonly ISmugetDbContext _dbContext;

    public AddIncomeCommandHandler(ISmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(AddIncomeCommand command, CancellationToken cancellationToken = default)
    {
        var monthlyBilling = await _dbContext.MonthlyBillings
            .Include(i => i.Incomes)
            .FirstOrDefaultAsync(
                m => m.Id.Value == command.MonthlyBillingId,
                cancellationToken
            );

        if (monthlyBilling is null)
        {
            throw new MonthlyBillingNotFoundException();
        }

        var income = new Income(
            new Name(command.Name),
            new Money(command.Amount, command.Currency),
            command.Include
        );

        monthlyBilling.AddIncome(income);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}