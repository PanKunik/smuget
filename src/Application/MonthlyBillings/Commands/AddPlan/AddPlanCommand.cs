using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.AddPlan;

public sealed record AddPlanCommand(
    Guid MonthlyBillingId,
    string Category,
    decimal MoneyAmount,
    string Currency,
    uint SortOrder
) : ICommand;