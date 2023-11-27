namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class PiggyBank
    {
        public static readonly Guid Id = new Guid(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public const string Name = "New car";
        public const bool WithGoal = true;
        public const decimal Goal = 15_000m;
    }
}