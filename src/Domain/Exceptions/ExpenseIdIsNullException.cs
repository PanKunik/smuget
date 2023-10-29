using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class ExpenseIdIsNullException
    : RequiredFieldException
{
    public ExpenseIdIsNullException()
        : base(nameof(ExpenseId)) { }
}