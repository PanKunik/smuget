namespace Domain.Exceptions;

public sealed class InvalidUserIdException : SmugetException
{
    public InvalidUserIdException()
        : base("User id cannot be empty.") { }
}
