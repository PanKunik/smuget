using Application.Abstractions.CQRS;
using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Commands.AddPlan;

public sealed record AddPlanCommand(
    Guid MonthlyBillingId,
    string Category,
    decimal MoneyAmount,
    Currency Currency,
    uint SortOrder
) : ICommand;