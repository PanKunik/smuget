using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.OpenMonthlyBilling;

public sealed record OpenMonthlyBillingCommand(
    ushort Year,
    byte Month) : ICommand;