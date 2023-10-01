using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record Category
{
    private const byte MaxLengthForCategoryName = 30;

    public string Value { get; }

    public Category(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new CategoryIsEmptyException();
        }

        if (value.Length > MaxLengthForCategoryName)
        {
            throw new CategoryIsTooLongException(
                value.Length,
                MaxLengthForCategoryName
            );
        }

        Value = value;
    }
}