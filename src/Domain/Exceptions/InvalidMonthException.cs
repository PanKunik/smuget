namespace Domain.Exceptions;

public sealed class InvalidMonthException : SmugetException
{
    public int Value { get; }

    public InvalidMonthException(int value)
        : base($"Month must be an integer number from 1 to 12. Passed: {value}.")
    {
        Value = value;
    }
}