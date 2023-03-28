namespace WebAPI.MonthlyBillings;

public sealed record OpenMonthlyBillingRequest(
    int Year,
    int Month);