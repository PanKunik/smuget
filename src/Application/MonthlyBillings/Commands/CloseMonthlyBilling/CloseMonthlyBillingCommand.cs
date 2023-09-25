using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.CloseMonthlyBilling;

public sealed record CloseMonthlyBillingCommand(
    int Year,
    int Month
) : ICommand;