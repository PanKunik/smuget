namespace Domain.Exceptions;

public sealed class InvalidRefreshTokenIdException : SmugetException
{
    public InvalidRefreshTokenIdException()
        : base("Refresh token cannot be empty.") { }
}
