namespace WebAPI.MonthlyBillings;

/// <summary>
/// Informations about new plan in monthly billing.
/// </summary>
/// <param name="Category">Tag name for the plan.</param>
/// <param name="Money">Amount of the plan.</param>
/// <param name="Currency">Currency of the plan.</param>
/// <param name="SortOrder">Order number.</param>
public sealed record AddPlanRequest(
    string Category,
    decimal Money,
    string Currency,
    uint SortOrder
);