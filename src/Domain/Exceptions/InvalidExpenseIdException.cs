using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class InvalidExpenseIdException
    : RequiredFieldException
{
    public InvalidExpenseIdException()
        : base(nameof(ExpenseId)) { }
}