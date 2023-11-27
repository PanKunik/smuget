namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class Transaction
    {
        public static readonly Guid Id = new(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public const decimal Value = 120.75m;
        public static readonly DateOnly Date = new(2023, 11, 11);

        public static Guid IdFromIndex(int index)
            => new(0, 0, 0, new byte[8] { 0, 0, 0, 0, 0, 0, 0, (byte)(index + 1)});

        public static decimal ValueFromIndex(int index)
            => 
            index % 2 == 0
            ? Value * (index + 1)
            : Value * -(index + 1);

        public static DateOnly DateFromIndex(int index)
            => new DateOnly(2023, 11, 11)
                .AddDays(index + 1);
    }
}