using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.AddPlan;

public sealed class AddPlanCommandHandler : ICommandHandler<AddPlanCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public AddPlanCommandHandler(IMonthlyBillingsRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(AddPlanCommand command, CancellationToken cancellationToken = default)
    {
        var monthlyBilling = await _repository.GetById(
            new MonthlyBillingId(command.MonthlyBillingId)
        );

        if (monthlyBilling is null)
        {
            throw new MonthlyBillingNotFoundException();
        }

        var plan = new Plan(
            new PlanId(Guid.NewGuid()),
            new Category(command.Category),
            new Money(
                command.MoneyAmount,
                new Currency(command.Currency)
            ),
            command.SortOrder
        );

        monthlyBilling.AddPlan(plan);
        await _repository.Save(monthlyBilling);
    }
}
