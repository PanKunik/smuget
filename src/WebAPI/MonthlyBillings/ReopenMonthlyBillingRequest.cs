using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

/// <summary>
/// Informations about monthly billing you try to reopen.
/// </summary>
/// <param name="Year">Year of the monthly billing.</param>
/// <param name="Month">Month of the monthly billing.</param>
public sealed record ReopenMonthlyBillingRequest(
    [FromRoute(Name = "year")] ushort Year,
    [FromRoute(Name = "month")] byte Month
);