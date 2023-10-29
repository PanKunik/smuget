using Application.Exceptions;

namespace Infrastructure.Exceptions;

public sealed class InvalidAccessTokenException
    : IdentityException
{
    public InvalidAccessTokenException(string message)
        : base(message) { }
}