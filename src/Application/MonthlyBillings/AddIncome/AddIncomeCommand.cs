using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.AddIncome;

public sealed record AddIncomeCommand(
    Guid MonthlyBillingId,
    string Name,
    decimal Amount,
    string Currency,
    bool Include
) : ICommand;