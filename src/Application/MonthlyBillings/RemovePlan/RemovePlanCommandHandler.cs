using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.RemovePlan;

public sealed class RemovePlanCommandHandler : ICommandHandler<RemovePlanCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public RemovePlanCommandHandler(IMonthlyBillingsRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(
        RemovePlanCommand command,
        CancellationToken cancellationToken = default
    )
    {
        MonthlyBillingId monthlyBillingId = new(command.MonthlyBillingId);
        var entity = await _repository.GetById(monthlyBillingId)
            ?? throw new MonthlyBillingNotFoundException();

        PlanId PlanId = new(command.PlanId);
        entity.RemovePlan(PlanId);

        await _repository.Save(entity);
    }
}
