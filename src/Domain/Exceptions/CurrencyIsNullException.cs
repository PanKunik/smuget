namespace Domain.Exceptions;

public sealed class CurrencyIsNullException : SmugetException
{
    public CurrencyIsNullException()
        : base("Currency cannot be null or empty.") { }
}