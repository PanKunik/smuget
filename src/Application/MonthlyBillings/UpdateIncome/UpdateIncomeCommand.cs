using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.UpdateIncome;

public sealed record UpdateIncomeCommand(
    Guid MonthlyBillingId,
    Guid IncomeId,
    string Name,
    decimal MoneyAmount,
    string Currency,
    bool Include
) : ICommand;