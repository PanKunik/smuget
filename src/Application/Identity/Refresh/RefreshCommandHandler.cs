using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Exceptions;
using Domain.RefreshTokens;
using Domain.Repositories;

namespace Application.Identity.Refresh;

public sealed class RefreshCommandHandler : ICommandHandler<RefreshCommand>
{
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;

    public RefreshCommandHandler(
        IRefreshTokensRepository repository,
        IUsersRepository usersRepository,
        IAuthenticator authenticator,
        ITokenStorage tokenStorage
    )
    {
        _refreshTokensRepository = repository;
        _usersRepository = usersRepository;
        _authenticator = authenticator;
        _tokenStorage = tokenStorage;
    }

    public async Task HandleAsync(
        RefreshCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _refreshTokensRepository.Get(
            command.RefreshToken
        ) ?? throw new RefreshTokenNotFoundException();

        if (entity.WasUsed)
        {
            throw new RefreshTokenUsedException();
        }

        if (entity.Expires < DateTime.UtcNow)
        {
            throw new RefreshTokenExpiredException();
        }

        var userEntity = await _usersRepository.Get(entity.UserId)
            ?? throw new UserNotFoundException();

        var token = _authenticator.CreateToken(userEntity.Id.Value);

        var refreshToken = new RefreshToken(
            new(Guid.NewGuid()),
            token.RefreshToken,
            DateTime.UtcNow.AddHours(2),
            false,
            userEntity.Id
        );
        await _refreshTokensRepository.Save(refreshToken);

        entity.Use();
        await _refreshTokensRepository.Save(entity);
        _tokenStorage.Store(token);
    }
}
