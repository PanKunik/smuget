namespace Domain.Exceptions;

public sealed class FirstNameIsNullException : SmugetException
{
    public FirstNameIsNullException()
        : base("First name is cannot be null or empty.") { }
}