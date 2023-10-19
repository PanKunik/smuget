using Domain.Exceptions;

namespace Application.Exceptions;

public abstract class IdentityException : SmugetException
{
    public IdentityException(string message)
        : base(message) { }
}