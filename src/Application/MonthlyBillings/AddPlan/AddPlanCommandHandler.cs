using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.AddPlan;

public sealed class AddPlanCommandHandler
    : ICommandHandler<AddPlanCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public AddPlanCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        AddPlanCommand command,
        CancellationToken cancellationToken = default
    )
    {
        MonthlyBillingId monthlyBillingId = new(command.MonthlyBillingId);

        var entity = await _repository.GetById(
                monthlyBillingId,
                new(command.UserId)
            )
            ?? throw new MonthlyBillingNotFoundException(monthlyBillingId);

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
