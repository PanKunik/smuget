using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class ExpenseNotFoundException
    : NotFoundException
{
    public ExpenseNotFoundException(ExpenseId expenseId)
        : base(
            nameof(Expense),
            nameof(ExpenseId),
            expenseId.Value.ToString()
        ) { }
}
