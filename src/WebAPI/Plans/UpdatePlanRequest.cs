namespace WebAPI.Plans;

/// <summary>
/// Updated values for plan in monthly billing.
/// </summary>
/// <param name="Category">New category name for plan.</param>
/// <param name="MoneyAmount">Updated value of money amount.</param>
/// <param name="Currency">New currency for money.</param>
/// <param name="SortOrder">New sort order value.</param>
public sealed record UpdatePlanRequest(
    string Category,
    decimal MoneyAmount,
    string Currency,
    uint SortOrder
);