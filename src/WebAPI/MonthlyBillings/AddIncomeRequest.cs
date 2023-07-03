namespace WebAPI.MonthlyBillings;

public sealed record AddIncomeRequest(
    string Name,
    decimal MoneyAmount,
    string Currency,
    bool IncludeInBilling = true);