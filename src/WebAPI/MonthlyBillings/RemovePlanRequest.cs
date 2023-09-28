using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

/// <summary>
/// Identificators for selecting monthly billing and plan id.
/// </summary>
/// <param name="MonthlyBillingId">Identifier of a monthly billing.</param>
/// <param name="PlanId">Identifier of a plan to remove.</param>
public sealed record RemovePlanRequest(
    [FromRoute(Name = "monthlyBillingId")] Guid MonthlyBillingId,
    [FromRoute(Name = "planId")] Guid PlanId
);