using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.ReopenMonthlyBilling;

public sealed record ReopenMonthlyBillingCommand(
    ushort Year,
    byte Month
) : ICommand;