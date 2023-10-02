namespace Domain.Exceptions;

public sealed class InvalidIncomeIdException : SmugetException
{
    public InvalidIncomeIdException()
        : base("Income id cannot be empty.") { }
}