using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class MonthIsNullException
    : RequiredFieldException
{
    public MonthIsNullException()
        : base(nameof(Month)) { }
}