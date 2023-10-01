using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record PlanId
{
    public Guid Value { get; }

    public PlanId(
        Guid value
    )
    {
        if (value == Guid.Empty)
        {
            throw new InvalidPlanIdException();
        }

        Value = value;
    }
}