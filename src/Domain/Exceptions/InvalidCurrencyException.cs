namespace Domain.Exceptions;

public sealed class InvalidCurrencyException : SmugetException
{
    public string Currency { get; }

    public InvalidCurrencyException(
        string currency
    )
        : base($"There is no currency like '{currency}' supported in application.")
    {
        Currency = currency;
    }
}