using Application.Abstractions.Security;
using Application.Exceptions;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Application.Users.Register;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.Users.Register;

public sealed class RegisterCommandHandlerTests
{
    private readonly IUsersRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly RegisterCommandHandler _cut;

    public RegisterCommandHandlerTests()
    {
        _repository = Substitute.For<IUsersRepository>();

        _repository
            .GetByEmail(new(Constants.User.Email))
            .Returns(UsersUtilities.CreateUser());

        _passwordHasher = Substitute.For<IPasswordHasher>();

        _passwordHasher
            .Secure(new(Constants.User.Password))
            .Returns(Constants.User.SecuredPassword);

        _cut = new RegisterCommandHandler(
            _repository,
            _passwordHasher
        );
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByEmailOnRepositoryOnce()
    {
        // Arrange
        var register = RegisterCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            register,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .GetByEmail(
                Arg.Is<Email>(e => e.Value == register.Email)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenUserWithSameEmailFound_ShouldThrowUserWithSameEmailAlreadyExistsException()
    {
        // Arrange
        var register = new RegisterCommand(
            "regular-user@smuget.com",
            Constants.User.FirstName,
            Constants.User.Password
        );

        var registerAction = () => _cut.HandleAsync(
            register,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<UserWithSameEmailAlreadyExistsException>(registerAction);
    }

    [Fact]
    public async Task HandleAsync_WhenEmailIsUnique_ShouldCallSecureOnPasswordHasherOnce()
    {
        // Arrange
        var register = RegisterCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            register,
            default
        );

        // Assert
        _passwordHasher
            .Received(1)
            .Secure(
                Arg.Is<string>(p => p == register.Password)
            );
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var register = RegisterCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            register,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<User>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainExpectedValues()
    {
        // Arrange
        var register = RegisterCommandUtilities.CreateCommand();

        User passedArgument = null;

        await _repository
            .Save(Arg.Do<User>(u => passedArgument = u));

        // Act
        await _cut.HandleAsync(
            register,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?
            .Should()
            .Match<User>(
                u => u.Email.Value == register.Email
                  && u.FirstName.Value == register.FirstName
            );
    }
}