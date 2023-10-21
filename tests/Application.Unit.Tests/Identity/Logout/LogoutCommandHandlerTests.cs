using Application.Exceptions;
using Application.Identity.Logout;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.RefreshTokens;
using Domain.Repositories;

namespace Application.Unit.Tests.Identity.Logout;

public sealed class LogoutCommandHandlerTests
{
    private readonly IRefreshTokensRepository _refreshTokensRepository;

    private readonly LogoutCommandHandler _cut;

    public LogoutCommandHandlerTests()
    {
        _refreshTokensRepository = Substitute.For<IRefreshTokensRepository>();

        _refreshTokensRepository
            .GetActiveByUserId(Constants.User.Id)
            .Returns(RefreshTokensUtilities.CreateRefreshToken());

        _cut = new LogoutCommandHandler(
            _refreshTokensRepository
        );
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetActiveByUserIdOnRefreshTokensRepositoryOnce()
    {
        // Arrange
        var command = LogoutCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            command,
            default
        );

        // Assert
        await _refreshTokensRepository
            .Received(1)
            .GetActiveByUserId(
                Constants.User.Id
            );
    }

    [Fact]
    public async Task HandleAsync_WhenRefreshTokenWasntFound_ShouldThrowUserAlreadyLoggedOutException()
    {
        // Arrange
        var command = new LogoutCommand(
            "MXi0AcV-+==",
            Guid.Empty
        );

        var commandAction = () => _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyLoggedOutException>(commandAction);
    }

    [Fact]
    public async Task HandleAsync_WhenFoundRefreshTokenDoesntMatchProvidedRefreshToken_ShouldThrowInvalidRefreshTokenException()
    {
        // Arrange
        var command = new LogoutCommand(
            "MXi0AcV-+==",
            Constants.User.Id
        );

        var commandAction = () => _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(commandAction);
    }

    [Fact]
    public async Task HandleAsync_WhenRefreshTokenWasFound_ShouldCallSaveOnRefreshTokensRepositoryOnce()
    {
        // Arrange
        var command = LogoutCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            command,
            default
        );

        // Assert
        await _refreshTokensRepository
            .Received(1)
            .Save(
                Arg.Is<RefreshToken>(
                    r => r.Token == Constants.RefreshToken.Token
                      && r.Invalidated == true
                )
            );
    }
}