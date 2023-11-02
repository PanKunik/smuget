using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.UpdateIncome;

public sealed class UpdateIncomeCommandHandler
    : ICommandHandler<UpdateIncomeCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public UpdateIncomeCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        UpdateIncomeCommand command,
        CancellationToken cancellationToken = default
    )
    {
        MonthlyBillingId monthlyBillingId = new(command.MonthlyBillingId);

        var entity = await _repository.GetById(
                monthlyBillingId,
                new(command.UserId)
            )
            ?? throw new MonthlyBillingNotFoundException(monthlyBillingId);

        entity.UpdateIncome(
            new(command.IncomeId),
            new(command.Name),
            new(
                command.MoneyAmount,
                new(command.Currency)
            ),
            command.Include
        );
        await _repository.Save(entity);
    }
}
