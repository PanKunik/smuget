namespace Domain.Exceptions;

public sealed class InvalidYearException : SmugetException
{
    public int Value { get; }

    public InvalidYearException(int value)
        : base($"Year must be an integer number greater than 1900. Passed: {value}.")
    {
        Value = value;
    }
}