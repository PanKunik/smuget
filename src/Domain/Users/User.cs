using Domain.Exceptions;

namespace Domain.Users;

public sealed class User
{
    public UserId UserId { get; }
    public Email Email { get; }
    public FirstName FirstName { get; }
    public Password Password { get; }

    public User(
        UserId userId,
        Email email,
        FirstName firstName,
        Password password
    )
    {
        UserId = userId ?? throw new UserIdIsNullException();
        Email = email ?? throw new EmailIsNullException();
        FirstName = firstName ?? throw new FirstNameIsNullException();
        Password = password ?? throw new PasswordIsNullException();
    }
}