using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.Repositories;

namespace Application.MonthlyBillings.RemoveExpense;

public sealed class RemoveExpenseCommandHandler : ICommandHandler<RemoveExpenseCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public RemoveExpenseCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        RemoveExpenseCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _repository.GetById(
            new(command.MonthlyBillingId),
            new(command.UserId)
        ) ?? throw new MonthlyBillingNotFoundException();

        entity.RemoveExpense(
            new(command.PlanId),
            new(command.ExpenseId)
        );
        await _repository.Save(entity);
    }
}
