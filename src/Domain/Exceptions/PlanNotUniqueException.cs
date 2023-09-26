namespace Domain.Exceptions;

public sealed class PlanCategoryNotUniqueException : SmugetException
{
    public string Category { get; }

    public PlanCategoryNotUniqueException(
        string category
    )
        : base($"Plan's category `{category}` already exists in monthly billing.")
    {
        Category = category;
    }

}