using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Exceptions;
using Domain.Repositories;
using Domain.Users;

namespace Application.Identity.Register;

public sealed class RegisterCommandHandler
    : ICommandHandler<RegisterCommand>
{
    private readonly IUsersRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IUsersRepository repository,
        IPasswordHasher passwordHasher
    )
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _passwordHasher = passwordHasher
            ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task HandleAsync(
        RegisterCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var email = new Email(command.Email);
        var password = new Password(command.Password);
        var firstName = new FirstName(command.FirstName);

        var entity = await _repository.GetByEmail(email);

        if (entity is not null)
        {
            throw new UserWithSameEmailAlreadyExistsException(email);
        }

        var securedPassword = _passwordHasher.Secure(password.Value);

        var user = new User(
            new(Guid.NewGuid()),
            email,
            firstName,
            securedPassword
        );

        await _repository.Save(user);
    }
}
