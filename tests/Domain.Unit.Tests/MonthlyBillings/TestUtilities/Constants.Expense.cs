using System;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.TestUtilities;

public static partial class Constants
{
    public static class Expense
    {
        public static readonly ExpenseId Id = new(Guid.NewGuid());
        public static readonly DateTimeOffset ExpenseDate = new(new DateTime(2023, 6, 15));
        public static readonly Money Money = new(45.67M, new Currency("PLN"));
        public const string Descripiton = "Description";

        public static Money MoneyFromIndex(int index)
            => new(
                Money.Amount * (index + 1),
                Money.Currency
            );

        public static DateTimeOffset ExpenseDateFromIndex(int index)
            => ExpenseDate.AddDays(index);

        public static string DescripitonFromIndex(int index)
            => $"{Descripiton} {index}";
    }
}