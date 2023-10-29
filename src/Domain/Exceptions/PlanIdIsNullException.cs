using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class PlanIdIsNullException
    : RequiredFieldException
{
    public PlanIdIsNullException()
        : base(nameof(PlanId)) { }
}