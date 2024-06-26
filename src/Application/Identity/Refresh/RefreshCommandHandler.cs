using Application.Abstractions.Authentication;
using Application.Abstractions.CQRS;
using Application.Abstractions.Time;
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
    private readonly IClock _clock;

    public RefreshCommandHandler(
        IUsersRepository repository,
        IRefreshTokensRepository refreshTokensRepository,
        IAuthenticator authenticator,
        ITokenStorage tokenStorage,
        IClock clock
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
        _clock = clock
            ?? throw new ArgumentNullException(nameof(clock));
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

        if (!existingRefreshToken.IsActive())
        {
            await RevokeDescendantRefreshTokens(existingRefreshToken);
            throw new InvalidRefreshTokenException("Refresh token was used or invalidated. Invalidating all descendant refresh tokens.");
        }

        if (_clock.Current() > existingRefreshToken.ExpirationDateTime)
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
            command.IpAddess,
            false,
            false,
            user.Id,
            existingRefreshToken.Id
        );

        existingRefreshToken.Use();
        await _refreshTokensRepository.Save(existingRefreshToken);
        await _refreshTokensRepository.Save(refreshTokenEntity);
        _tokenStorage.Store(token);
    }

    private async Task RevokeDescendantRefreshTokens(RefreshToken potentiallyCompromisedToken)
    {
        if (!potentiallyCompromisedToken.Invalidated)
        {
            potentiallyCompromisedToken.Invalidate();
            await _refreshTokensRepository.Save(potentiallyCompromisedToken);
        }

        var getDescendantRefreshToken = await _refreshTokensRepository.GetByRefreshedBy(potentiallyCompromisedToken.Id.Value);
        if (getDescendantRefreshToken is null)
        {
            return;
        }

        await RevokeDescendantRefreshTokens(getDescendantRefreshToken);
    }
}
