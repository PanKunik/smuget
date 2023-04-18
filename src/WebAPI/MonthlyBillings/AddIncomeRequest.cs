using Domain.MonthlyBillings;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

public sealed record AddIncomeRequest(
    string Name,
    decimal MoneyAmount,
    Currency Currency,
    bool IncludeInBilling = true);