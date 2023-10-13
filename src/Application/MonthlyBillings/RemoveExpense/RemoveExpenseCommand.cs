using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.RemoveExpense;

public sealed record RemoveExpenseCommand(
    Guid MonthlyBillingId,
    Guid PlanId,
    Guid ExpenseId,
    Guid UserId
) : ICommand;