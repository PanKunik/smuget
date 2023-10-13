using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.UpdatePlan;

public sealed record UpdatePlanCommand(
    Guid MonthlyBillingId,
    Guid PlanId,
    string Category,
    decimal MoneyAmount,
    string Currency,
    uint SortOrder,
    Guid UserId
) : ICommand;