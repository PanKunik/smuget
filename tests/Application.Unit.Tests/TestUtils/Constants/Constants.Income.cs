namespace Application.Unit.Tests.TestUtils.Constants;

public static partial class Constants
{
    public static class Income
    {
        public static readonly Guid Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public const string Name = "Earn";
        public const decimal Amount = 4321.45m;
        public const string Currency = "PLN";
        public const bool Include = true;
    }
}