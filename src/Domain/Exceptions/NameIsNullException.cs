using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class NameIsNullException
    : RequiredFieldException
{
    public NameIsNullException()
        : base(nameof(Name)) { }
}