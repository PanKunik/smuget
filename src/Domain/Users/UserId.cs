using Domain.Exceptions;

namespace Domain.Users;

public sealed record UserId
{
    public Guid Value { get; }

    public UserId(Guid value)
    {
        ThrowIfValueEqualsEmptyGuid(value);
        Value = value;
    }

    private void ThrowIfValueEqualsEmptyGuid(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidUserIdException();
        }
    }
}