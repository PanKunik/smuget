namespace Domain.Exceptions;

public sealed class InvalidTokenException : SmugetException
{
    public InvalidTokenException()
        : base("Token cannot be null or empty.") { }
}