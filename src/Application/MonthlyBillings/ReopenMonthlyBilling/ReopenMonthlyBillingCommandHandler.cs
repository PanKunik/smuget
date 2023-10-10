using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.ReopenMonthlyBilling;

public sealed class ReopenMonthlyBillingCommandHandler : ICommandHandler<ReopenMonthlyBillingCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public ReopenMonthlyBillingCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository;
    }

    public async Task HandleAsync(
        ReopenMonthlyBillingCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var year = new Year(command.Year);
        var month = new Month(command.Month);

        var monthlyBilling = await _repository.Get(
            year,
            month
        );

        if (monthlyBilling is null)
        {
            throw new MonthlyBillingNotFoundException(
                (ushort)year.Value,
                (byte)month.Value
            );
        }

        monthlyBilling.Reopen();
        await _repository.Save(monthlyBilling);
    }
}
