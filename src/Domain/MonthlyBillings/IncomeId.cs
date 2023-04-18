using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record IncomeId
{
    public Guid Value { get; }

    public IncomeId(
        Guid value
    )
    {
        if (value == Guid.Empty)
        {
            throw new InvalidIncomeIdException();
        }

        Value = value;
    }
}