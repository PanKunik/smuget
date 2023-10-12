using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.UpdateExpense;

public sealed record UpdateExpenseCommand(
    Guid MonthlyBillingId,
    Guid PlanId,
    Guid ExpenseId,
    decimal MoneyAmount,
    string Currency,
    DateTimeOffset ExpenseDate,
    string Description,
    Guid UserId
) : ICommand;