using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class NameIsToLongException
    : InvalidFieldLengthException
{
    public NameIsToLongException(byte maxLength)
        : base(
            nameof(Name),
            maxLength
        ) { }
}