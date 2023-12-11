using Application.Exceptions;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Application.Identity;
using Application.Identity.Refresh;
using Domain.RefreshTokens;
using Domain.Repositories;
using Domain.Users;
using Application.Abstractions.Authentication;
using Application.Abstractions.Time;

namespace Application.Unit.Tests.Identity.Refresh;

public sealed class RefreshCommandHandlerTests
{
	private readonly IRefreshTokensRepository _refreshTokensRepository;
	private readonly IUsersRepository _usersRepository;
	private readonly IAuthenticator _authenticator;
	private readonly ITokenStorage _tokenStorage;
	private readonly IClock _clock;

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
			.Get("invalidated-token")
			.Returns(RefreshTokensUtilities.CreateInvalidatedRefreshToken());

		_refreshTokensRepository
			.Get("token-for-other-user")
			.Returns(RefreshTokensUtilities.CreateForeignRefreshToken());

		_usersRepository = Substitute.For<IUsersRepository>();

		_usersRepository
			.Get(new(Constants.User.Id))
			.Returns(UsersUtilities.CreateUser());

		_authenticator = Substitute.For<IAuthenticator>();

		_authenticator
			.RefreshToken(
				Arg.Is<User>(
					u => u.Id.Value == Constants.User.Id
				),
				Constants.User.AccessToken,
				Constants.RefreshToken.JwtId
			)
			.Returns(
				new AuthenticationDTO()
				{
					Id = Guid.NewGuid(),
					AccessToken = Constants.User.AccessToken,
					RefreshToken = Constants.RefreshToken.Token,
					CreationDateTime = Constants.RefreshToken.CreationDateTime,
					ExpirationDateTime = Constants.RefreshToken.ExpirationDateTime,
				}
			);

		_tokenStorage = Substitute.For<ITokenStorage>();

		_clock = Substitute.For<IClock>();
		_clock
			.Current()
			.Returns(DateTime.Now);

		_cut = new RefreshCommandHandler(
			_usersRepository,
			_refreshTokensRepository,
			_authenticator,
			_tokenStorage,
			_clock
		);
	}

	[Fact]
	public async Task HandleAsync_WhenInvoked_ShouldCallGetOnRefreshTokensRepositoryOnce()
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
			.Get(
				Arg.Is<string>(
					a => a == Constants.RefreshToken.Token
				)
			);
	}

	[Fact]
	public async Task HandleAsync_WhenTokenDoesntExist_ShouldThrowInvalidRefreshTokenException()
	{
		// Arrange
		var command = new RefreshCommand(
			"abc123",
			"123def"
		);

		var commandAction = () => _cut.HandleAsync(
			command,
			default
		);

		// Act & Assert
		await Assert.ThrowsAsync<InvalidRefreshTokenException>(commandAction);
	}

	[Fact]
	public async Task HandleAsync_WhenRefreshTokenWasFound_ShouldCallGetOnUsersRepositoryOnce()
	{
		// Arrange
		var command = RefreshCommandUtilities.CreateCommand();

		// Act
		await _cut.HandleAsync(
			command,
			default
		);

		// Assert
		await _usersRepository
			.Received(1)
			.Get(
				Arg.Is<UserId>(
					u => u.Value == Constants.User.Id
				)
			);
	}

	[Fact]
	public async Task HandleAsync_WhenUserDoesntExist_ShouldThrowUserNotFoundException()
	{
		// Arrange
		var command = new RefreshCommand(
			Constants.User.AccessToken,
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
	public async Task HandleAsync_WhenTokenWasUsedBefore_ShouldThrowInvalidRefreshTokenException()
	{
		// Arrange
		var command = new RefreshCommand(
			Constants.User.AccessToken,
			"used-token"
		);

		var commandAction = () => _cut.HandleAsync(
			command,
			default
		);

		// Act & Assert
		await Assert.ThrowsAsync<InvalidRefreshTokenException>(commandAction);
	}

	[Fact]
	public async Task HandleAsync_WhenTokenWasInvalidatedBefore_ShouldThrowInvalidRefreshTokenException()
	{
		// Arrange
		var command = new RefreshCommand(
			Constants.User.AccessToken,
			"invalidated-token"
		);

		var commandAction = () => _cut.HandleAsync(
			command,
			default
		);

		// Act & Assert
		await Assert.ThrowsAsync<InvalidRefreshTokenException>(commandAction);
	}

	[Fact]
	public async Task HandleAsync_WhenTokenHasExpired_ShouldThrowInvalidRefreshTokenException()
	{
		// Arrange
		var command = new RefreshCommand(
			Constants.User.AccessToken,
			"expired-token"
		);

		var commandAction = () => _cut.HandleAsync(
			command,
			default
		);

		// Act & Assert
		await Assert.ThrowsAsync<InvalidRefreshTokenException>(commandAction);
	}

	[Fact]
	public async Task HandleAsync_WhenRefreshTokenIsValid_ShouldCallRefreshTokenOnAuthenticatorOnce()
	{
		// Arrange
		var command = RefreshCommandUtilities.CreateCommand();

		// Act
		await _cut.HandleAsync(
			command,
			default
		);

		// Assert
		_authenticator
			.Received(1)
			.RefreshToken(
				Arg.Is<User>(
					u => u.Id.Value == Constants.User.Id
				),
				command.AccessToken,
				Arg.Any<Guid>()
			);
	}

	[Fact]
	public async Task HandleAsync_WhenTokenCreatedSuccessfully_ShouldCallSaveOnRefreshTokenRepositoryTwice()
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
			.Received(2)
			.Save(
				Arg.Any<RefreshToken>()
			);

		await _refreshTokensRepository
			.Received(1)
			.Save(
				Arg.Is<RefreshToken>(
					c => c.Used == true
				)
			);

		await _refreshTokensRepository
			.Received(1)
			.Save(
				Arg.Is<RefreshToken>(
					c => c.Used == false
				)
			);
	}

	[Fact]
	public async Task HandleAsync_WhenTokenCreatedSuccessfully_ShouldCallStoreOnTokenStorageOnce()
	{
		// Arrange
		var command = RefreshCommandUtilities.CreateCommand();

		// Act
		await _cut.HandleAsync(
			command,
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