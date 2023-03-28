namespace Domain.Exceptions;

public sealed class InvalidEntityIdException : SmugetException
{
    public InvalidEntityIdException()
        : base("Entity id cannot be empty.")
    {
    }
}