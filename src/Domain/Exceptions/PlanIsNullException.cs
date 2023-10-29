using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class PlanIsNullException
    : RequiredFieldException
{
    public PlanIsNullException()
        : base(nameof(Plan)) { }
}