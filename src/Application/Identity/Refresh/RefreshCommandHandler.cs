using Application.Abstractions.Authentication;
using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.RefreshTokens;
using Domain.Repositories;

namespace Application.Identity.Refresh;

public sealed class RefreshCommandHandler
    : ICommandHandler<RefreshCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;

    public RefreshCommandHandler(
        IUsersRepository repository,
        IRefreshTokensRepository refreshTokensRepository,
        IAuthenticator authenticator,
        ITokenStorage tokenStorage
    )
    {
        _usersRepository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _refreshTokensRepository = refreshTokensRepository
            ?? throw new ArgumentNullException(nameof(refreshTokensRepository));
        _authenticator = authenticator
            ?? throw new ArgumentNullException(nameof(authenticator));
        _tokenStorage = tokenStorage
            ?? throw new ArgumentNullException(nameof(tokenStorage));
    }

    public async Task HandleAsync(
        RefreshCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var existingRefreshToken = await _refreshTokensRepository.Get(command.RefreshToken)
            ?? throw new InvalidRefreshTokenException("Refresh token wasn't found.");

        var user = await _usersRepository.Get(existingRefreshToken.UserId)
            ?? throw new UserNotFoundException(existingRefreshToken.UserId);

        if (existingRefreshToken.Used || existingRefreshToken.Invalidated)
        {
            throw new InvalidRefreshTokenException("Rfresh token was used or invalidated.");
        }

        if (DateTime.Now > existingRefreshToken.ExpirationDateTime)
        {
            throw new InvalidRefreshTokenException("Refresh token has expired.");
        }

        var token = _authenticator.RefreshToken(
            user,
            command.AccessToken,
            existingRefreshToken.JwtId
        );

        var refreshTokenEntity = new RefreshToken(
            new(Guid.NewGuid()),
            token.RefreshToken,
            token.Id,
            token.CreationDateTime,
            token.ExpirationDateTime,
            false,
            false,
            user.Id
        );

        existingRefreshToken.Use();
        await _refreshTokensRepository.Save(existingRefreshToken);
        await _refreshTokensRepository.Save(refreshTokenEntity);
        _tokenStorage.Store(token);
    }
}
