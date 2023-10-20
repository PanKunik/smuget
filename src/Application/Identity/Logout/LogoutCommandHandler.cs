using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.Repositories;

namespace Application.Identity.Logout;

public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IRefreshTokensRepository _refreshTokensRepository;

    public LogoutCommandHandler(
        IRefreshTokensRepository refreshTokensRepository
    )
    {
        _refreshTokensRepository = refreshTokensRepository;
    }

    public async Task HandleAsync(
        LogoutCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var existingRefreshToken = await _refreshTokensRepository.GetActiveByUserId(
            command.UserId
        ) ?? throw new UserAlreadyLoggedOutException();

        if (existingRefreshToken.Token != command.RefreshToken)
        {
            throw new InvalidRefreshTokenException("Refresh token is not equal to users refresh token.");
        }

        existingRefreshToken.Invalidate();
        await _refreshTokensRepository.Save(existingRefreshToken);
    }
}