using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.RemoveIncome;

public sealed record RemoveIncomeCommand(
    Guid MonthlyBillingId,
    Guid IncomeId,
    Guid UserId
) : ICommand;