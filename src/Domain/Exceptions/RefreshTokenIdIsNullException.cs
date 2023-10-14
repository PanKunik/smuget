namespace Domain.Exceptions;

public sealed class RefreshTokenIdIsNullException : SmugetException
{
    public RefreshTokenIdIsNullException()
        : base("Refresh token id cannot be null.") { }
}