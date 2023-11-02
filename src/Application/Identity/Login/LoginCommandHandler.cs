using Application.Abstractions.Authentication;
using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Exceptions;
using Domain.RefreshTokens;
using Domain.Repositories;
using Domain.Users;

namespace Application.Identity.Login;

public sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;

    public LoginCommandHandler(
        IUsersRepository repository,
        IPasswordHasher passwordHasher,
        IRefreshTokensRepository refreshTokensRepository,
        IAuthenticator authenticator,
        ITokenStorage tokenStorage
    )
    {
        _usersRepository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _passwordHasher = passwordHasher
            ?? throw new ArgumentNullException(nameof(passwordHasher));
        _refreshTokensRepository = refreshTokensRepository
            ?? throw new ArgumentNullException(nameof(refreshTokensRepository));
        _authenticator = authenticator
            ?? throw new ArgumentNullException(nameof(authenticator));
        _tokenStorage = tokenStorage
            ?? throw new ArgumentNullException(nameof(tokenStorage));
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

        var existingRefreshToken = await _refreshTokensRepository.GetActiveByUserId(entity.Id.Value);

        if (existingRefreshToken is not null)
        {
            throw new UserAlreadyLoggedInException();
        }

        var token = _authenticator.CreateToken(entity);

        var refreshTokenEntity = new RefreshToken(
            new(Guid.NewGuid()),
            token.RefreshToken,
            token.Id,
            token.CreationDateTime,
            token.ExpirationDateTime,
            false,
            false,
            entity.Id
        );

        await _refreshTokensRepository.Save(refreshTokenEntity);
        _tokenStorage.Store(token);
    }
}
