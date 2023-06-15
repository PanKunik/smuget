using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.TestUtilities;

public static partial class Constants
{
    public static class MonthlyBilling
    {
        public static readonly Year Year = new(2023);
        public static readonly Month Month = new(2);
        public static readonly State State = State.Open;
    }
}