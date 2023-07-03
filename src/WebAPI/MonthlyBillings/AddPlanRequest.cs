using Domain.MonthlyBillings;

namespace WebAPI.MonthlyBillings;

public sealed record AddPlanRequest(
    string Category,
    decimal Money,
    string Currency,
    uint SortOrder
);