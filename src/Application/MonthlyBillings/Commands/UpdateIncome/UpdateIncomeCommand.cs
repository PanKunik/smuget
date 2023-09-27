using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.UpdateIncome;

public sealed record UpdateIncomeCommand(
    Guid MonthlyBillingId,
    Guid IncomeId,
    string Name,
    decimal MoneyAmount,
    string Currency,
    bool Include
) : ICommand;