namespace Domain.Exceptions;

public sealed class UserIdIsNullException : SmugetException
{
    public UserIdIsNullException()
        : base("User id cannot be null or empty.") { }
}