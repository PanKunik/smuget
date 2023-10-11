using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Users.Login;
using Application.Users.Register;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Users;

namespace WebAPI.Unit.Tests.Users;

public sealed class UsersControllerTests
{
    private readonly UsersController _cut;

    private readonly Mock<ICommandHandler<RegisterCommand>> _mockRegister;
    private readonly Mock<ICommandHandler<LoginCommand>> _mockLogin;
    private readonly Mock<ITokenStorage> _mockTokenStorage;

    public UsersControllerTests()
    {
        _mockRegister = new Mock<ICommandHandler<RegisterCommand>>();
        _mockLogin = new Mock<ICommandHandler<LoginCommand>>();
        _mockTokenStorage = new Mock<ITokenStorage>();

        _cut = new UsersController(
            _mockRegister.Object,
            _mockLogin.Object,
            _mockTokenStorage.Object
        );
    }

    [Fact]
    public async Task Register_OnSuccess_ShouldReturnCreatedResult()
    {
        // Arrange
        var registerRequest = new RegisterRequest(
            "joedoe@test.com",
            "Joe",
            "abcd1234)(*&"
        );

        // Act
        var result = await _cut.Register(registerRequest);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task Register_OnSuccess_ShouldReturnStatusCode201()
    {
        // Arrange
        var registerRequest = new RegisterRequest(
            "joedoe@test.com",
            "Joe",
            "abcd1234)(*&"
        );

        // Act
        var result = (CreatedResult)await _cut.Register(registerRequest);

        // Assert
        result
            .Should()
            .NotBeNull();

        result.StatusCode
            .Should()
            .Be(201);
    }

    [Fact]
    public async Task Register_WhenInvoked_ShouldCallRegisterCommandHandler()
    {
        // Arrange
        var registerRequest = new RegisterRequest(
            "joedoe@test.com",
            "Joe",
            "abcd1234)(*&"
        );

        // Act
        await _cut.Register(registerRequest);

        // Assert
        _mockRegister
            .Verify(r => r.HandleAsync(
                It.IsAny<RegisterCommand>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
    }

    [Theory]
    [InlineData("joedoe@test.com", "Joe", "my-super-secret-password")]
    [InlineData("me-email@example.com", "Anthony", "P@s$w0rd.1!")]
    public async Task Register_WhenInvokedWithParameters_ShouldPassThemToCommand(
        string emailValue,
        string firstNameValue,
        string passwordValue
    )
    {
        // Arrange
        var registerRequest = new RegisterRequest(
            emailValue,
            firstNameValue,
            passwordValue
        );

        // Act
        await _cut.Register(registerRequest);

        // Assert
        _mockRegister
            .Verify(
                r => r.HandleAsync(
                    It.Is<RegisterCommand>(
                        c => c.Email == emailValue
                          && c.FirstName == firstNameValue
                          && c.Password == passwordValue
                    ),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
    }

    [Fact]
    public async Task Login_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        var request = new LoginRequest(
            "test2@example.com",
            "P@$sw0rd1."
        );

        // Act
        var result = await _cut.Login(request);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Login_OnSuccess_ShouldReturn200StatusCode()
    {
        // Arrange
        var request = new LoginRequest(
            "test2@example.com",
            "P@$sw0rd1."
        );

        // Act
        var result = (OkObjectResult)await _cut.Login(request);

        // Assert
        result.StatusCode
            .Should()
            .Be(200);
    }

    [Fact]
    public async Task Login_WhenInvoked_ShouldCallLoginCommandHandler()
    {
        // Arrange
        var request = new LoginRequest(
            "test2@example.com",
            "P@$sw0rd1."
        );

        // Act
        await _cut.Login(request);

        // Assert
        _mockLogin
            .Verify(
                l => l.HandleAsync(
                    It.IsAny<LoginCommand>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
    }

    [Fact]
    public async Task Login_WhenInvokedWithParameters_ShouldPassThemToCommand()
    {
        // Arrange
        var request = new LoginRequest(
            "test2@example.com",
            "P@$sw0rd1."
        );

        // Act
        await _cut.Login(request);

        // Assert
        _mockLogin
            .Verify(
                l => l.HandleAsync(
                    It.Is<LoginCommand>(
                        c => c.Email == request.Email
                          && c.Password == request.Password
                    ),
                    It.IsAny<CancellationToken>()
                )
            );
    }
}