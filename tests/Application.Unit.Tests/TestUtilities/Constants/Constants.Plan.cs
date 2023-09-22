using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class Plan
    {
        public static readonly Guid Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public const string Category = "Pay";
        public const decimal MoneyAmount = 2984.12m;
        public const string Currency = "PLN";
        public const uint SortOrder = 1;

        public static Category CategoryFromIndex(int index)
            => new($"{Category} {index}");

        public static Money MoneyFromIndex(int index)
            => new(
                MoneyAmount * (index + 1),
                new (Currency)
            );
    }
}