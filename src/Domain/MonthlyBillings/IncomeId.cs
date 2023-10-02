using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record IncomeId
{
    public Guid Value { get; }

    public IncomeId(Guid value)
    {
        ThrowIfValueEqualsEmptyGuid(value);
        Value = value;
    }

    private void ThrowIfValueEqualsEmptyGuid(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidIncomeIdException();
        }
    }
}