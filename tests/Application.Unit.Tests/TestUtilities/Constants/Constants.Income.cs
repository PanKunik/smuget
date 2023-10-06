using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class Income
    {
        public static readonly Guid Id = new Guid(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public const string Name = "Earn";
        public const decimal Amount = 4321.45m;
        public const string Currency = "PLN";
        public const bool Include = true;

        public static IncomeId IdFromIndex(byte index)
            => new(new Guid(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, (byte)(index + 1) }));

        public static Name NameFromIndex(byte index)
            => new($"{Name} {index}");

        public static Money MoneyFromIndex(byte index)
            => new(
                Amount * (index + 1),
                new(Currency)
            );

        public static bool IncludeFromIndex(byte index)
            => index % 2 == 0 ? Include : !Include;
    }
}