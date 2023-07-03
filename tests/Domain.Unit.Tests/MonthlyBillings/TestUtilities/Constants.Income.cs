using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.TestUtilities;

public static partial class Constants
{
    public static class Income
    {
        public static readonly Name Name = new("Name");
        public static readonly Money Money = new(11.75M, new Currency("PLN"));

        public static Name NameFromIndex(int index)
            => new($"{Name} {index}");

        public static Money MoneyFromIndex(int index)
            => new(
                Money.Amount * index,
                Money.Currency
            );
    }
}