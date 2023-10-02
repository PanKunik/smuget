using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.UpdateExpense;

public sealed record UpdateExpenseCommand(
    Guid MonthlyBillingId,
    Guid PlanId,
    Guid ExpenseId,
    decimal MoneyAmount,
    string Currency,
    DateTimeOffset ExpenseDate,
    string Description
) : ICommand;