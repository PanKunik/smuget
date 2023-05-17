using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class Category
{
    private const byte MaxLengthForCategoryName = 20;
    public string Value { get; }

    public Category(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new CategoryIsNullException();    // TODO: Change name
        }

        Value = value;
    }

    private Category() { }
}