using Application.Abstractions.CQRS;
using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Commands.AddExpense;

public sealed record AddExpenseCommand(
    Guid MonthlyBillingId,
    Guid PlanId,
    decimal Money,
    string Currency,
    DateTimeOffset ExpenseDate,
    string Description
) : ICommand;