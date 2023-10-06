using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class MonthlyBilling
    {
        public static readonly Guid Id = new Guid(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public const ushort Year = 2023;
        public const byte Month = 7;
        public const string Currency = "PLN";
        public static readonly State State = State.Open;
    }
}