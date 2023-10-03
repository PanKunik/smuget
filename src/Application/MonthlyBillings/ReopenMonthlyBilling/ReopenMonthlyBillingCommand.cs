using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.ReopenMonthlyBilling;

public sealed record ReopenMonthlyBillingCommand(
    ushort Year,
    byte Month
) : ICommand;