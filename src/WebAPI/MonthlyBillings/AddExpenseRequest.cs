using Domain.MonthlyBillings;

namespace WebAPI.MonthlyBillings;

public sealed record AddExpenseRequest(
    decimal Money,
    string Currency,
    DateTimeOffset ExpenseDate,
    string Description
);