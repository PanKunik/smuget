using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.RemoveExpense;

public sealed record RemoveExpenseCommand(
    Guid MonthlyBillingId,
    Guid PlanId,
    Guid ExpenseId
) : ICommand;