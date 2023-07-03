namespace Domain.Exceptions;

public sealed class NameIsNullException : SmugetException
{
    public NameIsNullException()
        : base("Name cannot be null.") { }
}