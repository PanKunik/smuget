namespace WebAPI.MonthlyBillings;

/// <summary>
/// Updated values for income in monthly billing.
/// </summary>
/// <param name="Name">New name for income.</param>
/// <param name="MoneyAmount">Updated value of money amount.</param>
/// <param name="Currency">New currency for money.</param>
/// <param name="Include">New include value.</param>
public sealed record UpdateIncomeRequest(
    string Name,
    decimal MoneyAmount,
    string Currency,
    bool Include
);