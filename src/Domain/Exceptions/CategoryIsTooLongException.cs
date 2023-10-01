namespace Domain.Exceptions;

public sealed class CategoryIsTooLongException
    : SmugetException
{
    public CategoryIsTooLongException(int length, int maxLength)
        : base($"Name is too long. It can contains max. {maxLength} characters. You passed: {length} characters.")
    {
    }
}
