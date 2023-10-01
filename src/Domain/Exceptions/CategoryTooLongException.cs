namespace Domain.Exceptions;

public sealed class CategoryTooLongException
    : SmugetException
{
    public CategoryTooLongException(int maxLength)
        : base($"Category can have maximum { maxLength } characters.")
    {
    }
}
