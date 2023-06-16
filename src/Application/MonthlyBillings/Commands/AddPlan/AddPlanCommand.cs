using Application.Abstractions.CQRS;
using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Commands.AddPlan;

public sealed record AddPlanCommand(
    Guid MonthlyBillingId,
    string Category,
    decimal MoneyAmount,
    string Currency,
    uint SortOrder
) : ICommand;