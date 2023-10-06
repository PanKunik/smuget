using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class Expense
    {
        public static readonly Guid Id = new Guid(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public const decimal Money = 928.54m;
        public const string Currency = "PLN";
        public static readonly DateTimeOffset ExpenseDate = new DateTimeOffset(new DateTime(2023, 9, 15), new TimeSpan(1, 0, 0));
        public const string Description = "A short description about the expense";

        public static ExpenseId IdFromIndex(byte index)
            => new(new Guid(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, (byte)(index + 1) }));

        public static Money MoneyFromIndex(byte index)
            => new(
                Money * (index + 1),
                new(Currency)
            );

        public static DateTimeOffset ExpenseDateFromIndex(byte index)
            => ExpenseDate.AddDays(index + 1);

        public static string DescriptionFromIndex(byte index)
            => $"{Description} {index}";
    }
}