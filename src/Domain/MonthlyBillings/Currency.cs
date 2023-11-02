using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class Currency
{
    private readonly string[] _currencies = { "PLN", "USD", "EUR" };

    public string Value { get; }

    public Currency(string value)
    {
        ThrowIfValueIsNullOrWhiteSpace(value);
        ThrowIfValueIsNotAvailable(value);
        Value = value;
    }

    private void ThrowIfValueIsNullOrWhiteSpace(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new CurrencyIsNullException();
        }
    }

    private void ThrowIfValueIsNotAvailable(string value)
    {
        if (!_currencies.Contains(value))
        {
            throw new InvalidCurrencyException(value);
        }
    }

    public override string ToString()
    {
        return Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        return ((Currency)obj).Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(
        Currency left,
        Currency right
    )
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        Currency left,
        Currency right
    )
    {
        return !Equals(left, right);
    }

    public bool Equals(Currency other)
    {
        return Equals((object?)other);
    }
}