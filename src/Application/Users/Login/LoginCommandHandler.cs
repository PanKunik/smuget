using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Exceptions;
using Domain.Repositories;
using Domain.Users;

namespace Application.Users.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand>
{
    private readonly IUsersRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;

    public LoginCommandHandler(
        IUsersRepository repository,
        IPasswordHasher passwordHasher,
        IAuthenticator authenticator,
        ITokenStorage tokenStorage
    )
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _authenticator = authenticator;
        _tokenStorage = tokenStorage;
    }

    public async Task HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default
    )
    {
        Email email = new(command.Email);
        var entity = await _repository.GetByEmail(email)
            ?? throw new InvalidCredentialsException();

        if (!_passwordHasher.Validate(command.Password, entity.SecuredPassword))
        {
            throw new InvalidCredentialsException();
        }

        var token = _authenticator.CreateToken(entity.Id.Value);
        _tokenStorage.Store(token);
        // TODO: Save refresh token in database
    }
}
