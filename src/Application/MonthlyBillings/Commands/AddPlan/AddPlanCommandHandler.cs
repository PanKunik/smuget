using Application.Abstractions.CQRS;
using Application.Abstractions.Persistance;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;

namespace Application.MonthlyBillings.Commands.AddPlan;

public sealed class AddPlanCommandHandler : ICommandHandler<AddPlanCommand>
{
    private readonly ISmugetDbContext _dbContext;

    public AddPlanCommandHandler(ISmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(AddPlanCommand command, CancellationToken cancellationToken = default)
    {
        var monthlyBilling = await _dbContext.MonthlyBillings
            .Include(m => m.Incomes)
            .Include(m => m.Plans)
            .FirstOrDefaultAsync(m => m.Id.Value == command.MonthlyBillingId);

        if (monthlyBilling is null)
        {
            throw new MonthlyBillingNotFoundException();
        }

        if (monthlyBilling.State == State.Closed)
        {
            throw new MonthlyBillingAlreadyClosedException(
                monthlyBilling.Month,
                monthlyBilling.Year);
        }

        var plan = new Plan(
            new Category(command.Category),
            new Money(command.MoneyAmount, command.Currency),
            command.SortOrder
        );

        monthlyBilling.AddPlan(plan);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
