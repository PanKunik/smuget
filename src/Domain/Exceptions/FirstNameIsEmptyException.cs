namespace Domain.Exceptions;

public sealed class FirstNameIsEmptyException : SmugetException
{
    public FirstNameIsEmptyException() 
        : base("First name cannot be empty.") { }
}
