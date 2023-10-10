using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Exceptions;
using Domain.Repositories;
using Domain.Users;

namespace Application.Users.Register;

public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IUserRepository repository,
        IPasswordHasher passwordHasher
    )
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
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
            throw new UserWithSameEmailAlreadyExistsException();
        }

        var securedPassword = await _passwordHasher.Secure(password);

        var user = new User(
            new(Guid.NewGuid()),
            email,
            firstName,
            securedPassword
        );

        await _repository.Save(user);
    }
}
