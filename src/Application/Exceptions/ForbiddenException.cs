namespace Application.Exceptions;

public abstract class ForbiddenException
    : IdentityException
{
    public ForbiddenException(string message)
        : base(message) { }
}