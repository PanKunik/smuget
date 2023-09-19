namespace Application.Unit.Tests.TestUtils.Constants;

public static partial class Constants
{
    public static class Plan
    {
        public static readonly Guid Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public const string Category = "Pay";
        public const decimal MoneyAmount = 2984.12m;
        public const string Currency = "PLN";
        public const uint SortOrder = 1;
    }
}