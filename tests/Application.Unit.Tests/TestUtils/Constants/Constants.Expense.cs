namespace Application.Unit.Tests.TestUtils.Constants;

public static partial class Constants
{
    public static class Expense
    {
        public static readonly Guid Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public const decimal Money = 928.54m;
        public const string Currency = "PLN";
        public static readonly DateTimeOffset ExpenseDate = new DateTimeOffset(new DateTime(2023, 9, 15), new TimeSpan(1, 0, 0));
        public const string Description = "A short description about the expense";
    }
}