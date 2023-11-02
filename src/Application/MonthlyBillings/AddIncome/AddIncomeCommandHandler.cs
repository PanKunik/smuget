using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.AddIncome;

public sealed class AddIncomeCommandHandler
    : ICommandHandler<AddIncomeCommand>
{
    private readonly IMonthlyBillingsRepository _repository;

    public AddIncomeCommandHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        AddIncomeCommand command,
        CancellationToken cancellationToken = default
    )
    {
        MonthlyBillingId monthlyBillingId = new(command.MonthlyBillingId);

        var entity = await _repository.GetById(
                monthlyBillingId,
                new(command.UserId)
            )
            ?? throw new MonthlyBillingNotFoundException(monthlyBillingId);

        var income = new Income(
            new(Guid.NewGuid()),
            new(command.Name),
            new(
                command.Amount,
                new(command.Currency)
            ),
            command.Include
        );

        entity.AddIncome(income);
        await _repository.Save(entity);
    }
}