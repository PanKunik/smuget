using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class CategoryIsTooLongException
    : InvalidFieldLengthException
{
    public CategoryIsTooLongException(int maxLength)
        : base(
            nameof(Category),
            maxLength
        ) { }
}
