using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.AddPlan;

public sealed class AddPlanCommandHandler : ICommandHandler<AddPlanCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public AddPlanCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        AddPlanCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _repository.GetById(
            new(command.MonthlyBillingId),
            new(command.UserId)
        ) ?? throw new MonthlyBillingNotFoundException();

        var plan = new Plan(
            new(Guid.NewGuid()),
            new(command.Category),
            new(
                command.MoneyAmount,
                new(command.Currency)
            ),
            command.SortOrder
        );

        entity.AddPlan(plan);
        await _repository.Save(entity);
    }
}
