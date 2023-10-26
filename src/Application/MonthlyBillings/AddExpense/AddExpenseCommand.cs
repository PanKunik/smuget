using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.AddExpense;

public sealed record AddExpenseCommand(
    Guid MonthlyBillingId,
    Guid PlanId,
    decimal Money,
    string Currency,
    DateOnly ExpenseDate,
    string Description,
    Guid UserId
) : ICommand;