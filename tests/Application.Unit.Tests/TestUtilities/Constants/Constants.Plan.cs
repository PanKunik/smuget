using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class Plan
    {
        public static readonly Guid Id = new Guid(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public const string Category = "Pay";
        public const decimal MoneyAmount = 2984.12m;
        public const string Currency = "PLN";
        public const uint SortOrder = 1;

        public static PlanId IdFromIndex(byte index)
            => new(new Guid(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, (byte)(index + 1) }));

        public static Category CategoryFromIndex(byte index)
            => new($"{Category} {index}");

        public static Money MoneyFromIndex(byte index)
            => new(
                MoneyAmount * (index + 1),
                new(Currency)
            );

        public static uint SortOrderFromIndex(byte index)
            => SortOrder * (uint)(index + 1);
    }
}