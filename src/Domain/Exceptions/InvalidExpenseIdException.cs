namespace Domain.Exceptions;

public sealed class InvalidExpenseIdException : SmugetException
{
    public InvalidExpenseIdException()
        : base("Expense id cannot be empty.") { }
}