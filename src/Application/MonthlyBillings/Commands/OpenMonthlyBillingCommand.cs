using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands;

public sealed record OpenMonthlyBillingCommand(
    int Year,
    int Month) : ICommand;