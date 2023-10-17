using Application.Abstractions.Security;
using Application.Exceptions;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Application.Identity;
using Application.Identity.Refresh;
using Domain.RefreshTokens;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.Identity.Refresh;

public sealed class RefreshCommandHandlerTests
{
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;

    private readonly RefreshCommandHandler _cut;

    public RefreshCommandHandlerTests()
    {
        _refreshTokensRepository = Substitute.For<IRefreshTokensRepository>();

        _refreshTokensRepository
            .Get(Constants.RefreshToken.Token)
            .Returns(RefreshTokensUtilities.CreateRefreshToken());

        _refreshTokensRepository
            .Get("expired-token")
            .Returns(RefreshTokensUtilities.CreateExpiredRefreshToken());

        _refreshTokensRepository
            .Get("used-token")
            .Returns(RefreshTokensUtilities.CreateUsedRefreshToken());

        _refreshTokensRepository
            .Get("token-for-other-user")
            .Returns(RefreshTokensUtilities.CreateForeignRefreshToken());

        _usersRepository = Substitute.For<IUsersRepository>();

        _usersRepository
            .Get(new(Constants.User.Id))
            .Returns(UsersUtilities.CreateUser());

        _authenticator = Substitute.For<IAuthenticator>();

        _authenticator
            .CreateToken(UsersUtilities.CreateUser())
            .Returns(
                new AuthenticationDTO()
                {
                    AccessToken = Constants.User.AccessToken,
                    RefreshToken = Constants.RefreshToken.Token
                }
            );

        _tokenStorage = Substitute.For<ITokenStorage>();

        _cut = new RefreshCommandHandler(
            _refreshTokensRepository,
            _usersRepository,
            _authenticator,
            _tokenStorage
        );
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetOnRepositoryOnce()
    {
        // Arrange
        var command = RefreshCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            command,
            default
        );

        // Assert
        await _refreshTokensRepository
            .Received(1)
            .Get(Arg.Is<string>(a => a == Constants.RefreshToken.Token));
    }

    [Fact]
    public async Task HandleAsync_WhenTokenDoesntExist_ShouldThrowRefreshTokenNotFoundException()
    {
        // Arrange
        var command = new RefreshCommand(
            "abc123"
        );

        var commandAction = () => _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<RefreshTokenNotFoundException>(commandAction);
    }

    [Fact]
    public async Task HandleAsync_WhenTokenWasUsedBefore_ShouldThrowRefreshTokenUsedException()
    {
        // Arrange
        var command = new RefreshCommand(
            "used-token"
        );

        var commandAction = () => _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<RefreshTokenUsedException>(commandAction);
    }

    [Fact]
    public async Task HandleAsync_WhenTokenHasExpired_ShouldThrowRefreshTokenExpiredException()
    {
        // Arrange
        var command = new RefreshCommand(
            "expired-token"
        );

        var commandAction = () => _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<RefreshTokenExpiredException>(commandAction);
    }

    [Fact]
    public async Task HandleAsync_WhenUserDOesntExist_ShouldThrowUserNotFoundException()
    {
        // Arrange
        var command = new RefreshCommand(
            "token-for-other-user"
        );

        var commandAction = () => _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(commandAction);
    }

    [Fact]
    public async Task HandleAsync_WhenTokenAndUserWasFound_ShouldCallCreateTokenOnAuthenticatorOnce()
    {
        // Arrange
        var login = RefreshCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            login,
            default
        );

        // Assert
        _authenticator
            .Received(1)
            .CreateToken(
                Arg.Is<User>(
                    u => u.Id.Value == Constants.User.Id
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenTokenCreatedSuccessfully_ShouldCallSaveOnRefreshTokenRepositoryOnce()
    {
        // Arrange
        var login = RefreshCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            login,
            default
        );

        // Assert
        await _refreshTokensRepository
            .Received(1)
            .Save(
                Arg.Is<RefreshToken>(
                    r => r.UserId.Value == Constants.User.Id
                      && r.Token == Constants.RefreshToken.Token
                      && r.Used == false
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenTokenCreatedSuccessfully_ShouldCallStoreOnTokenStorageOnce()
    {
        // Arrange
        var login = RefreshCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            login,
            default
        );

        // Assert
        _tokenStorage
            .Received(1)
            .Store(
                Arg.Is<AuthenticationDTO>(
                    a => a.AccessToken == Constants.User.AccessToken
                      && a.RefreshToken == Constants.RefreshToken.Token
                )
            );
    }
}