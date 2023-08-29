using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.Commands.OpenMonthlyBilling;

public sealed class OpenMonthlyBillingCommandHandler : ICommandHandler<OpenMonthlyBillingCommand>
{
    private readonly IMonthlyBillingRepository _repository;

    public OpenMonthlyBillingCommandHandler(IMonthlyBillingRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(OpenMonthlyBillingCommand command, CancellationToken cancellationToken = default)
    {
        var year = new Year(command.Year);
        var month = new Month(command.Month);
        var currency = new Currency(command.Currency);

        var existingMonthlyBilling = await _repository.Get(
            year,
            month
        );

        if (existingMonthlyBilling is not null)
        {
            throw new MonthlyBillingAlreadyOpenedException(
                month.Value,
                year.Value
            );
        }

        var monthlyBilling = new MonthlyBilling(
            new MonthlyBillingId(Guid.NewGuid()),
            year,
            month,
            currency,
            State.Open,
            null,
            null
        );

        await _repository.Save(monthlyBilling);
    }
}