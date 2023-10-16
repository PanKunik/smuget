using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Users.Login;
using Application.Users.Refresh;
using Application.Users.Register;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebAPI.Users;

namespace WebAPI.Unit.Tests.Users;

public sealed class UsersControllerTests
{
    private readonly UsersController _cut;

    private readonly ICommandHandler<RegisterCommand> _mockRegister;
    private readonly ICommandHandler<LoginCommand> _mockLogin;
    private readonly ICommandHandler<RefreshCommand> _mockRefresh;
    private readonly ITokenStorage _mockTokenStorage;

    public UsersControllerTests()
    {
        _mockRegister = Substitute.For<ICommandHandler<RegisterCommand>>();
        _mockLogin = Substitute.For<ICommandHandler<LoginCommand>>();
        _mockRefresh = Substitute.For<ICommandHandler<RefreshCommand>>();
        _mockTokenStorage = Substitute.For<ITokenStorage>();

        _cut = new UsersController(
            _mockRegister,
            _mockLogin,
            _mockRefresh,
            _mockTokenStorage
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
        await _mockRegister
            .Received(1)
            .HandleAsync(
                Arg.Any<RegisterCommand>(),
                Arg.Any<CancellationToken>()
            );
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
        await _mockRegister
            .Received(1)
            .HandleAsync(
                Arg.Is<RegisterCommand>(
                    c => c.Email == emailValue
                      && c.FirstName == firstNameValue
                      && c.Password == passwordValue
                ),
                Arg.Any<CancellationToken>()
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
        await _mockLogin
            .Received(1)
            .HandleAsync(
                Arg.Any<LoginCommand>(),
                Arg.Any<CancellationToken>()
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
        await _mockLogin
            .Received(1)
            .HandleAsync(
                Arg.Is<LoginCommand>(
                    c => c.Email == request.Email
                      && c.Password == request.Password
                ),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task Refresh_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Act
        var result = await _cut.Refresh("EpN8h+GhVmOIPiY3Qdj73NEXDkUifMLTxYBB3DWArjt29VbRsu/4XaQQM/AWgx/aa6B3e1UMVHVpWgNRNQCYPBOVhm2jQMX3TSns8jgt/CoSOfEQNTsF9eVifoKSAVUU9hevr07Yv6SdTqHReoIOPsoNlfqgrwDCmZDOwjw4ABo");

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Refresh_OnSuccess_ShouldReturn200OkStatusCode()
    {
        // Act
        var result = (OkObjectResult)await _cut.Refresh("EpN8h+GhVmOIPiY3Qdj73NEXDkUifMLTxYBB3DWArjt29VbRsu/4XaQQM/AWgx/aa6B3e1UMVHVpWgNRNQCYPBOVhm2jQMX3TSns8jgt/CoSOfEQNTsF9eVifoKSAVUU9hevr07Yv6SdTqHReoIOPsoNlfqgrwDCmZDOwjw4ABo");

        // Assert
        result.StatusCode
            .Should()
            .Be(200);
    }

    [Fact]
    public async Task Refresh_WhenInvokedWithParameters_ShouldCallRefreshCommandHandlerWithParametersOnce()
    {
        // Act
        await _cut.Refresh("EpN8h+GhVmOIPiY3Qdj73NEXDkUifMLTxYBB3DWArjt29VbRsu/4XaQQM/AWgx/aa6B3e1UMVHVpWgNRNQCYPBOVhm2jQMX3TSns8jgt/CoSOfEQNTsF9eVifoKSAVUU9hevr07Yv6SdTqHReoIOPsoNlfqgrwDCmZDOwjw4ABo");

        // Assert
        await _mockRefresh
            .Received(1)
            .HandleAsync(
                Arg.Is<RefreshCommand>(
                    c => c.RefreshToken == "EpN8h+GhVmOIPiY3Qdj73NEXDkUifMLTxYBB3DWArjt29VbRsu/4XaQQM/AWgx/aa6B3e1UMVHVpWgNRNQCYPBOVhm2jQMX3TSns8jgt/CoSOfEQNTsF9eVifoKSAVUU9hevr07Yv6SdTqHReoIOPsoNlfqgrwDCmZDOwjw4ABo"
                ),
                Arg.Any<CancellationToken>()
            );
    }
}