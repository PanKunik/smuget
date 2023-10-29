using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class CategoryIsNullException
    : RequiredFieldException
{
    public CategoryIsNullException()
        : base(nameof(Category)) { }
}