using Application.Abstractions.Security;
using Application.Exceptions;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Application.Identity;
using Application.Identity.Login;
using Domain.RefreshTokens;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.Identity.Login;

public sealed class LoginCommandHandlerTests
{
    private readonly LoginCommandHandler _cut;

    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenStorage _tokenStorage;
    private readonly IRefreshTokensRepository _refreshTokenRepository;

    // public LoginCommandHandlerTests()
    // {
    //     _usersRepository = Substitute.For<IUsersRepository>();

    //     _usersRepository
    //         .GetByEmail(new(Constants.User.Email))
    //         .Returns(UsersUtilities.CreateUser());

    //     _passwordHasher = Substitute.For<IPasswordHasher>();

    //     _passwordHasher
    //         .Validate(
    //             Constants.User.Password,
    //             Constants.User.SecuredPassword
    //         )
    //         .Returns(true);

    //     _tokenStorage = Substitute.For<ITokenStorage>();
    //     _refreshTokenRepository = Substitute.For<IRefreshTokensRepository>();

    //     _cut = new LoginCommandHandler(
    //         _usersRepository,
    //         _passwordHasher,
    //         _authenticator,
    //         _tokenStorage,
    //         _refreshTokenRepository
    //     );

    // }

    // [Fact]
    // public async Task HandleAsync_WhenInvoked_ShouldCallGetByEmailOnRepositoryOnce()
    // {
    //     // Arrange
    //     var login = LoginCommandUtilities.CreateCommand();

    //     // Act
    //     await _cut.HandleAsync(
    //         login,
    //         default
    //     );

    //     // Assert
    //     await _usersRepository
    //         .Received(1)
    //         .GetByEmail(
    //             Arg.Is<Email>(
    //                 e => e.Value == login.Email
    //             )
    //         );
    // }

    // [Fact]
    // public async Task HandleAsync_WhenUserNotFound_ShouldThrowInvalidCredentialsException()
    // {
    //     // Arrange
    //     var login = new LoginCommand(
    //         "notfounduser@example.com",
    //         "P@$w0rd1."
    //     );

    //     var loginAction = () => _cut.HandleAsync(
    //         login,
    //         default
    //     );

    //     // Act & Assert
    //     await Assert.ThrowsAsync<InvalidCredentialsException>(loginAction);
    // }

    // [Fact]
    // public async Task HandleAsync_WhenUserFoundByEmail_ShouldCallValidateOnPasswordHasherOnce()
    // {
    //     // Arrange
    //     var login = LoginCommandUtilities.CreateCommand();

    //     // Act
    //     await _cut.HandleAsync(
    //         login,
    //         default
    //     );

    //     // Assert
    //     _passwordHasher
    //         .Received(1)
    //         .Validate(
    //             Arg.Is(Constants.User.Password),
    //             Arg.Is(Constants.User.SecuredPassword)
    //         );
    // }

    // [Fact]
    // public async Task HandleAsync_WhenPasswordIsInvalid_ShouldThrowInvalidCredentialsException()
    // {
    //     // Arrange
    //     var login = new LoginCommand(
    //         Constants.User.Email,
    //         "password"
    //     );

    //     var loginAction = () => _cut.HandleAsync(
    //         login,
    //         default
    //     );

    //     // Act & Assert
    //     await Assert.ThrowsAsync<InvalidCredentialsException>(loginAction);
    // }

    // [Fact]
    // public async Task HandleAsync_WhenUsersPasswordValidatedSuccessfully_ShouldCallCreateTokenOnAuthenticatorOnce()
    // {
    //     // Arrange
    //     var login = LoginCommandUtilities.CreateCommand();

    //     // Act
    //     await _cut.HandleAsync(
    //         login,
    //         default
    //     );

    //     // Assert
    //     _authenticator
    //         .Received(1)
    //         .CreateToken(
    //             Arg.Is<User>(
    //                 u => u.Id.Value == Constants.User.Id
    //             )
    //         );
    // }

    // [Fact]
    // public async Task HandleAsync_WhenTokenCreatedSuccessfully_ShouldCallSaveOnRefreshTokenRepositoryOnce()
    // {
    //     // Arrange
    //     var login = LoginCommandUtilities.CreateCommand();

    //     // Act
    //     await _cut.HandleAsync(
    //         login,
    //         default
    //     );

    //     // Assert
    //     await _refreshTokenRepository
    //         .Received(1)
    //         .Save(
    //             Arg.Is<RefreshToken>(
    //                 r => r.UserId.Value == Constants.User.Id
    //                   && r.Token == Constants.RefreshToken.Token
    //                   && r.Used == false
    //             )
    //         );
    // }

    // [Fact]
    // public async Task HandleAsync_WhenTokenCreatedSuccessfully_ShouldCallStoreOnTokenStorageOnce()
    // {
    //     // Arrange
    //     var login = LoginCommandUtilities.CreateCommand();

    //     // Act
    //     await _cut.HandleAsync(
    //         login,
    //         default
    //     );

    //     // Assert
    //     _tokenStorage
    //         .Received(1)
    //         .Store(
    //             Arg.Is<AuthenticationDTO>(
    //                 a => a.AccessToken == Constants.User.AccessToken
    //                   && a.RefreshToken == Constants.RefreshToken.Token
    //             )
    //         );
    // }
}