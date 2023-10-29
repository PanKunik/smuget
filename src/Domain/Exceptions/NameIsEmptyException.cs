using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class NameIsEmptyException
    : RequiredFieldException
{
    public NameIsEmptyException()
        : base(nameof(Name)) { }
}