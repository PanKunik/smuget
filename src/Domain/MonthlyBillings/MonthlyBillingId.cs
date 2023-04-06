using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record MonthlyBillingId
{
    public Guid Value { get; }

    public MonthlyBillingId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidMonthlyBillingIdException();
        }

        Value = value;
    }
}