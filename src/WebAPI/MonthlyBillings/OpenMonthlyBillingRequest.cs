namespace WebAPI.MonthlyBillings;

/// <summary>
/// Informations about monthly billing you try to open.
/// </summary>
/// <param name="Year">Year of the monthly billing.</param>
/// <param name="Month">Monthly of the monthly billing.</param>
/// <param name="Currency">Currency for monthly billing.</param>
public sealed record OpenMonthlyBillingRequest(
    ushort Year,
    byte Month,
    string Currency
);