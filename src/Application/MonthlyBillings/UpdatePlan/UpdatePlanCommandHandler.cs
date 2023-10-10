using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.UpdatePlan;

public sealed class UpdatePlanCommandHandler : ICommandHandler<UpdatePlanCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public UpdatePlanCommandHandler(IMonthlyBillingsRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(UpdatePlanCommand command, CancellationToken cancellationToken = default)
    {
        MonthlyBillingId monthlyBillingId = new(command.MonthlyBillingId);
        var entity = await _repository.GetById(monthlyBillingId)
            ?? throw new MonthlyBillingNotFoundException();

        entity.UpdatePlan(
            new(command.PlanId),
            new(command.Category),
            new(command.MoneyAmount, new(command.Currency)),
            command.SortOrder
        );

        await _repository.Save(entity);
    }
}
