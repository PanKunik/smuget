namespace Domain.Exceptions;

public sealed class NameIsEmptyException : SmugetException
{
    public NameIsEmptyException()
        : base($"Name of the income cannot be null or empty.") { }
}