using Domain.Users;

namespace Domain.Exceptions;

public sealed class EmailIsEmptyException
    : RequiredFieldException
{
    public EmailIsEmptyException()
        : base(nameof(Email)) { }
}