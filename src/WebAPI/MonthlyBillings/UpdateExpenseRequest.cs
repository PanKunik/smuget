namespace WebAPI.MonthlyBillings;

/// <summary>
/// Updated values for expense in monthly billing.
/// </summary>
/// <param name="MoneyAmount">Updated value of money amount.</param>
/// <param name="Currency">New currency for money.</param>
/// <param name="ExpenseDate">New expense date value.</param>
/// <param name="Description">New description.</param>
public sealed record UpdateExpenseRequest(
    decimal MoneyAmount,
    string Currency,
    DateTimeOffset ExpenseDate,
    string Description
);