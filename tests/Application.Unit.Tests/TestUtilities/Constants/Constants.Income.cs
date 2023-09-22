using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class Income
    {
        public static readonly Guid Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public const string Name = "Earn";
        public const decimal Amount = 4321.45m;
        public const string Currency = "PLN";
        public const bool Include = true;

        public static Name NameFromIndex(int index)
            => new($"{Name} {index}");

        public static Money MoneyFromIndex(int index)
            => new(
                Amount * (index + 1),
                new(Currency)
            );

        public static bool IncludeFromIndex(int index)
            => index % 2 == 0 ? Include : !Include;
    }
}