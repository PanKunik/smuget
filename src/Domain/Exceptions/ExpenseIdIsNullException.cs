namespace Domain.Exceptions;

public sealed class ExpenseIdIsNullException : SmugetException
{
    public ExpenseIdIsNullException()
        : base("ExpenseId cannot be null.") { }
}