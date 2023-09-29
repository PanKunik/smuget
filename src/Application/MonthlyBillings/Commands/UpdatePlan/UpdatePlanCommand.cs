using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.UpdatePlan;

public sealed record UpdatePlanCommand(
    Guid MonthlyBillingId,
    Guid PlanId,
    string Category,
    decimal MoneyAmount,
    string Currency,
    uint SortOrder
) : ICommand;