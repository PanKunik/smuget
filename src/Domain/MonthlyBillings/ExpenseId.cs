using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record ExpenseId
{
    public Guid Value { get; }

    public ExpenseId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidExpenseIdException();
        }

        Value = value;
    }
}