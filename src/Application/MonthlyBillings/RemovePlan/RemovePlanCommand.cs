using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.RemovePlan;

public sealed record RemovePlanCommand(
    Guid MonthlyBillingId,
    Guid PlanId
) : ICommand;