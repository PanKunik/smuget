using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands.RemovePlan;

public sealed record RemovePlanCommand(
    Guid MonthlyBillingId,
    Guid PlanId
) : ICommand;