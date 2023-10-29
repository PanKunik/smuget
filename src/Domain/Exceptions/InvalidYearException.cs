namespace Domain.Exceptions;

public sealed class InvalidYearException
    : ValidationException
{
    public int Year { get; }

    public InvalidYearException(int year)
        : base(
            $"Year must be an integer number greater than 1900. Passed: {year}.",
            nameof(Year)
        )
    {
        Year = year;
    }
}