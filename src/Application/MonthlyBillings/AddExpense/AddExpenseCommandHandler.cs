using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.AddExpense;

public sealed class AddExpenseCommandHandler : ICommandHandler<AddExpenseCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public AddExpenseCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        AddExpenseCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _repository.GetById(
            new(command.MonthlyBillingId),
            new(command.UserId)
        ) ?? throw new MonthlyBillingNotFoundException();

        var expense = new Expense(
            new(Guid.NewGuid()),
            new(
                command.Money,
                new(command.Currency)
            ),
            command.ExpenseDate,
            command.Description
        );

        entity.AddExpense(
            new(command.PlanId),
            expense
        );
        await _repository.Save(entity);
    }
}