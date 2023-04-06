namespace Domain.Exceptions;

public sealed class InvalidYearException : SmugetException
{
    public ushort Value { get; }

    public InvalidYearException(ushort value)
        : base($"Year must be an integer number greater than 1900. Passed: {value}.")
    {
        Value = value;
    }
}