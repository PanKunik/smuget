using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.RemoveIncome;

public sealed class RemoveIncomeCommandHandler : ICommandHandler<RemoveIncomeCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public RemoveIncomeCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        RemoveIncomeCommand command,
        CancellationToken cancellationToken = default
    )
    {
        MonthlyBillingId monthlyBillingId = new(command.MonthlyBillingId);

        var entity = await _repository.GetById(
            monthlyBillingId,
            new(command.UserId)
        ) ?? throw new MonthlyBillingNotFoundException(
            monthlyBillingId
        );

        entity.RemoveIncome(new(command.IncomeId));
        await _repository.Save(entity);
    }
}
