using Domain.MonthlyBillings;

namespace WebAPI.MonthlyBillings;

public sealed record AddIncomeRequest(
    string Name,
    decimal MoneyAmount,
    Currency Currency,
    bool IncludeInBilling = true);