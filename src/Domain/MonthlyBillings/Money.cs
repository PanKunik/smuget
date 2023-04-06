using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record Money
{
    public decimal Amount { get; }
    // TODO: Make currency own type - enum record
    public Currency Currency { get; }

    public Money(
        decimal amount,
        Currency currency
    )
    {
        Amount = amount;
        Currency = currency;
    }

    private Money() { }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new MoneyCurrencyMismatchException(
                left.Currency,
                right.Currency
            );
        }

        var sum = left.Amount + right.Amount;
        return new Money(sum, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new MoneyCurrencyMismatchException(
                left.Currency,
                right.Currency
            );
        }

        var difference = left.Amount - right.Amount;
        return new Money(difference, left.Currency);
    }
}