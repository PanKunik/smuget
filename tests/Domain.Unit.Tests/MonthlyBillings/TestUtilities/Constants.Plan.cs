using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.TestUtilities;

public static partial class Constants
{
    public static class Plan
    {
        public static readonly Category Category = new("Category");
        public static readonly Money Money = new(12.5M, Currency.PLN);

        public static Category CategoryFromIndex(int index)
            => new($"{Category.Value} {index}");

        public static Money MoneyFromIndex(int index)
            => new(
                Money.Amount * index,
                Money.Currency
            );
    }
}