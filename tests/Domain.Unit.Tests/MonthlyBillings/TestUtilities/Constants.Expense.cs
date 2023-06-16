using System;

namespace Domain.Unit.Tests.MonthlyBillings.TestUtilities;

public static partial class Constants
{
    public static class Expense
    {
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly DateTimeOffset ExpenseDate = new(new DateTime(2023, 6, 15));
        public const string Descripiton = "Description";
    }
}