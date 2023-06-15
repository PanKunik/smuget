using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

public sealed record GetMonthlyBillingRequest(
    [FromRoute(Name = "year")] ushort Year,
    [FromRoute(Name = "month")] byte Month
);