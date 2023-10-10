using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.UpdateExpense;

public sealed class UpdateExpenseCommandHandler : ICommandHandler<UpdateExpenseCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public UpdateExpenseCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository;
    }

    public async Task HandleAsync(UpdateExpenseCommand command, CancellationToken cancellationToken = default)
    {
        var monthlyBillingId = new MonthlyBillingId(command.MonthlyBillingId);
        var entity = await _repository.GetById(monthlyBillingId);

        if (entity is null)
        {
            throw new MonthlyBillingNotFoundException();
        }

        entity.UpdateExpense(
            new(command.PlanId),
            new(command.ExpenseId),
            new(command.MoneyAmount, new(command.Currency)),
            command.ExpenseDate,
            command.Description
        );

        await _repository.Save(entity);
    }
}
