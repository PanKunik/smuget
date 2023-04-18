using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class MoneyCurrencyMismatchException : SmugetException
{
    public Currency Left { get; }
    public Currency Right { get; }

    public MoneyCurrencyMismatchException(
        Currency left,
        Currency right
    ) : base($"Cannot add money in { nameof(left) } to money in { nameof(right) }.")
    {
        Left = left;
        Right = right;
    }
}