using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

/// <summary>
/// Identificators for selecting monthly billing and income id.
/// </summary>
/// <param name="MonthlyBillingId">Identifier of a monthly billing.</param>
/// <param name="IncomeId">Identifier of a income to remove.</param>
public sealed record RemoveIncomeRequest(
    [FromRoute(Name = "monthlyBillingId")] Guid MonthlyBillingId,
    [FromRoute(Name = "incomeId")] Guid IncomeId
);