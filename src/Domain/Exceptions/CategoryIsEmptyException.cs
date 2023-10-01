namespace Domain.Exceptions;

public sealed class CategoryIsEmptyException
    : SmugetException
{
    public CategoryIsEmptyException()
        : base("Category cannot be null or empty.")
    {
    }
}
