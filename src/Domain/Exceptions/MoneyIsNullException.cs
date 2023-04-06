namespace Domain.Exceptions;

public sealed class MoneyIsNullException : SmugetException
{
    public MoneyIsNullException()
        : base("Money cannot be null.") { }
}