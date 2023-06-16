using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class MonthlyBillingCurrencyMismatchException : SmugetException
{
    public Currency Currency { get; }

    public MonthlyBillingCurrencyMismatchException(
        Currency currency
    )
        : base($"The currency '{currency.Value}' doesn't match monthly billing's currency.")
    {
        Currency = currency;
    }
}