using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.CloseMonthlyBilling;

public sealed record CloseMonthlyBillingCommand(
    int Year,
    int Month,
    Guid UserId
) : ICommand;