using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

/// <summary>
/// Identifiers of a monthly billing, pland and expense id for expense removing.
/// </summary>
/// <param name="monthlyBillingId">Identifier for a monthly billing.</param>
/// <param name="planId">Identifier for a plan in monthly billing conatining the expense to remove.</param>
/// <param name="expenseId">Idenifier for a expense to remove.</param>
public sealed record RemoveExpenseRequest(
    [FromRoute(Name = "monthlyBillingId")] Guid monthlyBillingId,
    [FromRoute(Name = "planId")] Guid planId,
    [FromRoute(Name = "expenseId")] Guid expenseId
);