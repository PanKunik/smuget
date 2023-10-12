using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.Repositories;

namespace Application.MonthlyBillings.CloseMonthlyBilling;

public sealed class CloseMonthlyBillingCommandHandler : ICommandHandler<CloseMonthlyBillingCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public CloseMonthlyBillingCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        CloseMonthlyBillingCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _repository.Get(
            new(command.Year),
            new(command.Month),
            new(command.UserId)
        ) ?? throw new MonthlyBillingNotFoundException(
                (ushort)command.Year,
                (byte)command.Month
            );

        entity.Close();
        await _repository.Save(entity);
    }
}
