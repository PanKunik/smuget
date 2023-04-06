namespace Domain.Exceptions;

public sealed class YearIsNullException : SmugetException
{
    public YearIsNullException()
        : base("Year cannot be null or empty.") { }
}