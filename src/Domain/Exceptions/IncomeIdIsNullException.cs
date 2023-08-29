namespace Domain.Exceptions;

public sealed class IncomeIdIsNullException : SmugetException
{
    public IncomeIdIsNullException()
        : base("IncomeId cannot be null.") { }
}