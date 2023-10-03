using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.RemoveIncome;

public sealed class RemoveIncomeCommandHandler : ICommandHandler<RemoveIncomeCommand>
{
    private readonly IMonthlyBillingRepository _repository;

    public RemoveIncomeCommandHandler(IMonthlyBillingRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(
        RemoveIncomeCommand command,
        CancellationToken cancellationToken = default
    )
    {
        MonthlyBillingId monthlyBillingId = new(command.MonthlyBillingId);
        var entity = await _repository.GetById(monthlyBillingId)
            ?? throw new MonthlyBillingNotFoundException();

        IncomeId incomeId = new(command.IncomeId);
        entity.RemoveIncome(incomeId);

        await _repository.Save(entity);
    }
}
