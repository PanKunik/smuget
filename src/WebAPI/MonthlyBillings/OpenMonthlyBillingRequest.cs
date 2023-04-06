namespace WebAPI.MonthlyBillings;

public sealed record OpenMonthlyBillingRequest(
    ushort Year,
    byte Month);