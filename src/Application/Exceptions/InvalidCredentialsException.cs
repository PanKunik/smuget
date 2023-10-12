using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class InvalidCredentialsException : SmugetException
{
    public InvalidCredentialsException()
        : base("Invalid credentials.") { }
}