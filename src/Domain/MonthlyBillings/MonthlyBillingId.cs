using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record MonthlyBillingId
{
    public Guid Value { get; }

    public MonthlyBillingId(Guid value)
    {
        ThrowIfValueEqualsEmptyGuid(value);
        Value = value;
    }

    private void ThrowIfValueEqualsEmptyGuid(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidMonthlyBillingIdException();
        }
    }
}