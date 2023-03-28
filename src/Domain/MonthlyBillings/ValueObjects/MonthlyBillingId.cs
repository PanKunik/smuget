using Domain.Exceptions;

namespace Domain.MonthlyBillings.ValueObjects;

public sealed record MonthlyBillingId
{
    public Guid Value { get; }

    public MonthlyBillingId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException();
        }

        Value = value;
    }
}