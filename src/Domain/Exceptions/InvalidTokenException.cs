namespace Domain.Exceptions;

public sealed class InvalidTokenException
    : RequiredFieldException
{
    public InvalidTokenException()
        : base("token") { }
}