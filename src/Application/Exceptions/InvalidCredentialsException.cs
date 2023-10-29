namespace Application.Exceptions;

public sealed class InvalidCredentialsException
    : IdentityException
{
    public InvalidCredentialsException()
        : base("Invalid credentials.") { }
}