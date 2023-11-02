using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record Category
{
    private const byte MaxLengthForCategoryName = 30;

    public string Value { get; }

    public Category(string value)
    {
        ThrowIfValueIsNullOrWhiteSpace(value);
        ThrowIfValueIsLongerThanMaximumLength(value);
        Value = value;
    }

    private void ThrowIfValueIsNullOrWhiteSpace(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new CategoryIsEmptyException();
        }
    }

    private void ThrowIfValueIsLongerThanMaximumLength(string value)
    {
        if (value.Length > MaxLengthForCategoryName)
        {
            throw new CategoryIsTooLongException(MaxLengthForCategoryName);
        }
    }
}