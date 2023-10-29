using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class InvalidPlanIdException
    : RequiredFieldException
{
    public InvalidPlanIdException()
        : base(nameof(PlanId)) { }
}