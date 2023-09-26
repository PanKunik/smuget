using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

/// <summary>
/// Informations about monthly billing you try to close.
/// </summary>
/// <param name="Year">Year of the monthly billing.</param>
/// <param name="Month">Month of the monthly billing.</param>
public sealed record CloseMonthlyBillingRequest(
    [FromRoute(Name = "year")] ushort Year,
    [FromRoute(Name = "month")] byte Month
);