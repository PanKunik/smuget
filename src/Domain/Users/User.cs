using Domain.Exceptions;

namespace Domain.Users;

public sealed class User
{
    public UserId UserId { get; }
    public Email Email { get; }
    public FirstName FirstName { get; }
    public string SecuredPassword { get; }

    public User(
        UserId userId,
        Email email,
        FirstName firstName,
        string securedPassword
    )
    {
        UserId = userId ?? throw new UserIdIsNullException();
        Email = email ?? throw new EmailIsNullException();
        FirstName = firstName ?? throw new FirstNameIsNullException();
        
        ThrowIfPasswordIsNullOrWhiteSpace(securedPassword);
        SecuredPassword = securedPassword;
    }

    private void ThrowIfPasswordIsNullOrWhiteSpace(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new PasswordIsEmptyException();
        }
    }
}