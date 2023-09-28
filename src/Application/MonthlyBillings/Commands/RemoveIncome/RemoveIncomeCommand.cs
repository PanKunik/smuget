using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.RemoveIncome;

public sealed record RemoveIncomeCommand(
    Guid MonthlyBillingId,
    Guid IncomeId
) : ICommand;