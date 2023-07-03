using Application.Abstractions.CQRS;
using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Commands.AddIncome;

public sealed record AddIncomeCommand(
    Guid MonthlyBillingId,
    string Name,
    decimal Amount,
    string Currency,
    bool Include
) : ICommand;