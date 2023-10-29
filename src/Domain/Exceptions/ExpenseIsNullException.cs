using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class ExpenseIsNullException
    : RequiredFieldException
{
    public ExpenseIsNullException()
        : base(nameof(Expense)) { }
}