using Domain.Users;

namespace Domain.Exceptions;

public sealed class EmailIsNullException
    : RequiredFieldException
{
    public EmailIsNullException()
        : base(nameof(Email)) { }
}