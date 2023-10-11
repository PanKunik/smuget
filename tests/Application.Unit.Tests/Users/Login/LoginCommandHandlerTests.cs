using Application.Abstractions.Security;
using Application.Exceptions;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Application.Users.Login;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.Users.Login;

public sealed class LoginCommandHandlerTests
{
    private readonly LoginCommandHandler _cut;

    private readonly IUsersRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;

    public LoginCommandHandlerTests()
    {
        _repository = Substitute.For<IUsersRepository>();

        _repository
            .GetByEmail(new(Constants.User.Email))
            .Returns(UsersUtilities.CreateUser());

        _passwordHasher = Substitute.For<IPasswordHasher>();

        _passwordHasher
            .Validate(
                Constants.User.Password,
                Constants.User.SecuredPassword
            )
            .Returns(true);

        _authenticator = Substitute.For<IAuthenticator>();

        _authenticator
            .CreateToken(Constants.User.Id)
            .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");

        _tokenStorage = Substitute.For<ITokenStorage>();

        _cut = new LoginCommandHandler(
            _repository,
            _passwordHasher,
            _authenticator,
            _tokenStorage
        );

    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByEmailOnRepositoryOnce()
    {
        // Arrange
        var login = LoginCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            login,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .GetByEmail(
                Arg.Is<Email>(
                    e => e.Value == login.Email
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenUserNotFound_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var login = new LoginCommand(
            "notfounduser@example.com",
            "P@$w0rd1."
        );

        var loginAction = () => _cut.HandleAsync(
            login,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(loginAction);
    }

    [Fact]
    public async Task HandleAsync_WhenUserFoundByEmail_ShouldCallValidateOnPasswordHasherOnce()
    {
        // Arrange
        var login = LoginCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            login,
            default
        );

        // Assert
        _passwordHasher
            .Received(1)
            .Validate(
                Arg.Is(Constants.User.Password),
                Arg.Is(Constants.User.SecuredPassword)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenPasswordIsInvalid_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var login = new LoginCommand(
            Constants.User.Email,
            "password"
        );

        var loginAction = () => _cut.HandleAsync(
            login,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(loginAction);
    }

    [Fact]
    public async Task HandleAsync_WhenUsersPasswordValidatedSuccessfully_ShouldCallCreateTokenOnAuthenticatorOnce()
    {
        // Arrange
        var login = LoginCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            login,
            default
        );

        // Assert
        _authenticator
            .Received(1)
            .CreateToken(
                Arg.Is(Constants.User.Id)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenTokenCreatedSuccessfully_ShouldCallStoreOnTokenStorageOnce()
    {
        // Arrange
        var login = LoginCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            login,
            default
        );

        // Assert
        _tokenStorage
            .Received(1)
            .Store(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
            );
    }
}