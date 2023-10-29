using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class PlanNotFoundException
    : NotFoundException
{
    public PlanNotFoundException(PlanId planId)
        : base(
            nameof(Plan),
            nameof(PlanId),
            planId.Value.ToString()
        ) { }
}