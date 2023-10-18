using Domain.Exceptions;

namespace Infrastructure.Exceptions;

public abstract class IdentityException : SmugetException
{
    public IdentityException(string message)
        : base(message) { }
}