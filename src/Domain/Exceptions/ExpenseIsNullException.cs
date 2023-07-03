namespace Domain.Exceptions;

public sealed class ExpenseIsNullException : SmugetException
{
    public ExpenseIsNullException()
        : base("Expense cannot be empty.") { }
}