using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtils.Constants;

public static partial class Constants
{
    public static class MonthlyBilling
    {
        public static readonly Guid Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public const ushort Year = 2023;
        public const byte Month = 7;
        public const string Currency = "PLN";
        public static readonly State State = State.Open;
    }
}