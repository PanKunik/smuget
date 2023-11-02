using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.Repositories;

namespace Application.MonthlyBillings.ReopenMonthlyBilling;

public sealed class ReopenMonthlyBillingCommandHandler
    : ICommandHandler<ReopenMonthlyBillingCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public ReopenMonthlyBillingCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        ReopenMonthlyBillingCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _repository.Get(
                new(command.Year),
                new(command.Month),
                new(command.UserId)
            )
            ?? throw new MonthlyBillingNotFoundException(
                command.Year,
                command.Month
            );

        entity.Reopen();
        await _repository.Save(entity);
    }
}
