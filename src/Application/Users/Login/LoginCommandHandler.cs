using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Exceptions;
using Domain.RefreshTokens;
using Domain.Repositories;
using Domain.Users;

namespace Application.Users.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;
    private readonly IRefreshTokensRepository _refreshTokensRepository;

    public LoginCommandHandler(
        IUsersRepository repository,
        IPasswordHasher passwordHasher,
        IAuthenticator authenticator,
        ITokenStorage tokenStorage,
        IRefreshTokensRepository refreshTokenRepository
    )
    {
        _usersRepository = repository;
        _passwordHasher = passwordHasher;
        _authenticator = authenticator;
        _tokenStorage = tokenStorage;
        _refreshTokensRepository = refreshTokenRepository;
    }

    public async Task HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default
    )
    {
        Email email = new(command.Email);
        var entity = await _usersRepository.GetByEmail(email)
            ?? throw new InvalidCredentialsException();

        if (!_passwordHasher.Validate(command.Password, entity.SecuredPassword))
        {
            throw new InvalidCredentialsException();
        }

        var token = _authenticator.CreateToken(entity.Id.Value);

        var refreshToken = new RefreshToken(
            new(Guid.NewGuid()),
            token.RefreshToken,
            DateTime.UtcNow.AddHours(2),
            false,
            entity.Id
        );
        await _refreshTokensRepository.Save(refreshToken);
        _tokenStorage.Store(token);
    }
}
