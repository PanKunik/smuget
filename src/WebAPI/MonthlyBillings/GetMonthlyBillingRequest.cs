using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

/// <summary>
///  
/// </summary>
/// <param name="Year">Year of the monthly billing</param>
/// <param name="Month">Month of the monthly billing</param>
public sealed record GetMonthlyBillingRequest(
    [FromRoute(Name = "year")] ushort Year,
    [FromRoute(Name = "month")] byte Month
);