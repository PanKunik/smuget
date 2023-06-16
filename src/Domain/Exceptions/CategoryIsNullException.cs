namespace Domain.Exceptions;

public sealed class CategoryIsNullException : SmugetException
{
    public CategoryIsNullException()
        : base("Category cannot be null or empty.") { }
}