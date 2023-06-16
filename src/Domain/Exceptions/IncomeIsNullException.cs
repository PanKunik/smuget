namespace Domain.Exceptions;

public sealed class IncomeIsNullException : SmugetException
{
    public IncomeIsNullException()
        : base("Income cannot be empty.") { }
}