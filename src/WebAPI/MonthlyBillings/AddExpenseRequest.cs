using Domain.MonthlyBillings;

namespace WebAPI.MonthlyBillings;

public sealed record AddExpenseRequest(
    decimal Money,
    Currency Currency,
    DateTimeOffset ExpenseDate,
    string Description
);