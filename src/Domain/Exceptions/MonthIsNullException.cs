namespace Domain.Exceptions;

public sealed class MonthIsNullException : SmugetException
{
    public MonthIsNullException()
        : base("Month cannot be null or empty.") { }
}