using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class CategoryIsEmptyException
    : RequiredFieldException
{
    public CategoryIsEmptyException()
        : base(nameof(Category)) { }
}
