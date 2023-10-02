namespace Domain.Exceptions;

public sealed class ExpenseNotFoundException
    : SmugetException
{
    public ExpenseNotFoundException()
        : base("Expense with passed id doesn't exist in monthly billing.") { }
}
