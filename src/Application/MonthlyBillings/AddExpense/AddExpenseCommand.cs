using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.AddExpense;

public sealed record AddExpenseCommand(
    Guid MonthlyBillingId,
    Guid PlanId,
    decimal Money,
    string Currency,
    DateTimeOffset ExpenseDate,
    string Description
) : ICommand;