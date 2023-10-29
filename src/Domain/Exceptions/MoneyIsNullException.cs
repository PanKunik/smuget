using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class MoneyIsNullException
    : RequiredFieldException
{
    public MoneyIsNullException()
        : base(nameof(Money)) { }
}