using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;
using Domain.Users;

namespace Application.MonthlyBillings.OpenMonthlyBilling;

public sealed class OpenMonthlyBillingCommandHandler : ICommandHandler<OpenMonthlyBillingCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public OpenMonthlyBillingCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        OpenMonthlyBillingCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var year = new Year(command.Year);
        var month = new Month(command.Month);
        var currency = new Currency(command.Currency);
        var userId = new UserId(command.UserId);

        var existingMonthlyBilling = await _repository.Get(
            year,
            month,
            userId
        );

        if (existingMonthlyBilling is not null)
        {
            throw new MonthlyBillingAlreadyOpenedException(
                month.Value,
                year.Value
            );
        }

        var monthlyBilling = new MonthlyBilling(
            new(Guid.NewGuid()),
            year,
            month,
            currency,
            State.Open,
            userId
        );
        await _repository.Save(monthlyBilling);
    }
}