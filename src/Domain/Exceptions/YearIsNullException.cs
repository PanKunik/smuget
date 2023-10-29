using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class YearIsNullException
    : RequiredFieldException
{
    public YearIsNullException()
        : base(nameof(Year)) { }
}