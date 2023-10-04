using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.OpenMonthlyBilling;

public sealed record OpenMonthlyBillingCommand(
    ushort Year,
    byte Month,
    string Currency
) : ICommand;