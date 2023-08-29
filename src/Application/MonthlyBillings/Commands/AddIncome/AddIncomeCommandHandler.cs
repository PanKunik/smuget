using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.Commands.AddIncome;

public sealed class AddIncomeCommandHandler : ICommandHandler<AddIncomeCommand>
{
    private readonly IMonthlyBillingRepository _repository;

    public AddIncomeCommandHandler(IMonthlyBillingRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(AddIncomeCommand command, CancellationToken cancellationToken = default)
    {
        var monthlyBilling = await _repository.GetById(
            new MonthlyBillingId(command.MonthlyBillingId)
        );

        if (monthlyBilling is null)
        {
            throw new MonthlyBillingNotFoundException();
        }

        var income = new Income(
            new IncomeId(Guid.NewGuid()),
            new Name(command.Name),
            new Money(
                command.Amount,
                new Currency(command.Currency)
            ),
            command.Include
        );

        monthlyBilling.AddIncome(income);

        await _repository.Save(monthlyBilling);
    }
}