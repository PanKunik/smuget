using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.Commands.AddExpense;

public sealed class AddExpenseCommandHandler : ICommandHandler<AddExpenseCommand>
{
    private readonly IMonthlyBillingRepository _repository;

    public AddExpenseCommandHandler(IMonthlyBillingRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(AddExpenseCommand command, CancellationToken cancellationToken = default)
    {
        var monthlyBilling = await _repository.GetById(
            new MonthlyBillingId(command.MonthlyBillingId)
        );

        if (monthlyBilling is null)
        {
            throw new MonthlyBillingNotFoundException();
        }

        var expense = new Expense(
            new ExpenseId(Guid.NewGuid()),
            new Money(
                command.Money,
                new Currency(command.Currency)
            ),
            command.ExpenseDate,
            command.Description
        );

        monthlyBilling.AddExpense(
            new PlanId(command.PlanId),
            expense
        );

        await _repository.Save(monthlyBilling);
    }
}