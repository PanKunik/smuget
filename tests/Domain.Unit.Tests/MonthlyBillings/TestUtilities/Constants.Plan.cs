using System;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.TestUtilities;

public static partial class Constants
{
    public static class Plan
    {
        public static readonly PlanId Id = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        public static readonly Category Category = new("Category");
        public static readonly Money Money = new(12.5M, new Currency("PLN"));

        public static Category CategoryFromIndex(int index)
            => new($"{Category.Value} {index}");

        public static Money MoneyFromIndex(int index)
            => new(
                Money.Amount * (index + 1),
                Money.Currency
            );
    }
}