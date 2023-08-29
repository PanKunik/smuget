using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.Commands.AddPlan;

public sealed class AddPlanCommandHandler : ICommandHandler<AddPlanCommand>
{
    private readonly IMonthlyBillingRepository _repository;

    public AddPlanCommandHandler(IMonthlyBillingRepository repository)
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

        if (monthlyBilling.State == State.Closed)
        {
            throw new MonthlyBillingAlreadyClosedException(
                monthlyBilling.Month,
                monthlyBilling.Year);
        }   // TODO: Move to domain layer

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
