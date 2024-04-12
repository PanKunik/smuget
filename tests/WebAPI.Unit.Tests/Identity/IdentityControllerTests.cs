using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Authentication;
using Application.Abstractions.CQRS;
using Application.Identity;
using Application.Identity.Login;
using Application.Identity.Logout;
using Application.Identity.Refresh;
using Application.Identity.Register;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebAPI.Identity;
using WebAPI.Services.Users;

namespace WebAPI.Unit.Tests.Identity;

public sealed class IdentityControllerTests
{
    private readonly IdentityController _cut;

    private readonly ICommandHandler<RegisterCommand> _mockRegister;
    private readonly ICommandHandler<LoginCommand> _mockLogin;
    private readonly ICommandHandler<RefreshCommand> _mockRefresh;
    private readonly ICommandHandler<LogoutCommand> _mockLogout;
    private readonly ITokenStorage _mockTokenStorage;
    private readonly IUserService _mockUserService;

    public IdentityControllerTests()
    {
        _mockRegister = Substitute.For<ICommandHandler<RegisterCommand>>();
        _mockLogin = Substitute.For<ICommandHandler<LoginCommand>>();
        _mockRefresh = Substitute.For<ICommandHandler<RefreshCommand>>();
        _mockLogout = Substitute.For<ICommandHandler<LogoutCommand>>();

        _mockTokenStorage = Substitute.For<ITokenStorage>();
        _mockTokenStorage
            .Get()
            .Returns(
                new AuthenticationDTO()
                {
                    AccessToken = "eyJHc01b.",
                    RefreshToken = "HM3X=="
                }
            );

        _mockUserService = Substitute.For<IUserService>();
        _mockUserService.IpAddress
            .Returns("127.0.0.1");

        _cut = new IdentityController(
            _mockRegister,
            _mockLogin,
            _mockRefresh,
            _mockLogout,
            _mockTokenStorage,
            _mockUserService
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
    public async Task Register_WhenInvoked_ShouldCallHandleAsyncOnCommandHandlerOnce()
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
    public async Task Login_WhenIpAddressNotPresent_ShouldThrowSourceAddressNotFoundException()
    {
        // Arrange
        var request = new LoginRequest(
            "test2@example.com",
            "P@##w0rd1."
        );

        _mockUserService.IpAddress
            .Returns("");

        // Act & Assert
        var result = await Assert.ThrowsAsync<SourceAddressNotFoundException>(
            async () => await _cut.Login(request)
        );
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
    public async Task Login_WhenInvoked_ShouldCallHanelAsyncOnCommandHandlerOnce()
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

    [Theory]
    [InlineData("test2@example.com", "P@$sw0rd1.")]
    [InlineData("johndoe1@test.com", "!@#098asdCZ")]
    public async Task Login_WhenInvokedWithParameters_ShouldPassThemToCommand(
        string email,
        string password
    )
    {
        // Arrange
        var request = new LoginRequest(
            email,
            password
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
    public async Task Login_OnSucess_ShouldCallGetOnTokenStoargeOnce()
    {
        // Arrange
        var request = new LoginRequest(
            "test2@example.com",
            "P@$sw0rd1."
        );

        // Act
        await _cut.Login(request);

        // Assert
        _mockTokenStorage
            .Received(1)
            .Get();
    }

    [Fact]
    public async Task Refresh_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        var request = new RefreshRequest(
            "eyJHBn01.",
            "M8Xy+-=="
        );

        // Act
        var result = await _cut.Refresh(
            request
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task RefreshToken_WhenIpAddressNotPresent_ShouldThrowSourceAddressNotFoundException()
    {
        // Arrange
        var request = new RefreshRequest(
            "eyJHBn01.",
            "M8Xy+-=="
        );

        _mockUserService.IpAddress
            .Returns("");

        // Act & Assert
        var result = await Assert.ThrowsAsync<SourceAddressNotFoundException>(
            async () => await _cut.Refresh(request)
        );
    }

    [Fact]
    public async Task Refresh_OnSuccess_ShouldReturn200OkStatusCode()
    {
        // Arrange
        var request = new RefreshRequest(
            "eyJHBn01.",
            "M8Xy+-=="
        );

        // Act
        var result = (OkObjectResult)await _cut.Refresh(
            request
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(200);
    }

    [Fact]
    public async Task Refresh_WhenInvoked_ShouldCallHandleAsyncOnCommandHandlerOnce()
    {
        // Arrange
        var request = new RefreshRequest(
            "eyJHBn01.",
            "M8Xy+-=="
        );

        // Act
        await _cut.Refresh(
            request
        );

        // Assert
        await _mockRefresh
            .Received(1)
            .HandleAsync(
                Arg.Any<RefreshCommand>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Theory]
    [InlineData("eyJHBn01.", "M8Xy+-==")]
    [InlineData("amcn!3213.@d", "Xi*jhG%gt")]
    public async Task Refresh_WhenInvokedWithParameters_ShouldPassThemToCommand(
        string accessToken,
        string refreshToken
    )
    {
        // Arrange
        var request = new RefreshRequest(
            accessToken,
            refreshToken
        );

        // Act
        await _cut.Refresh(
            request
        );

        // Assert
        await _mockRefresh
            .Received(1)
            .HandleAsync(
                Arg.Is<RefreshCommand>(
                    c => c.AccessToken == accessToken
                      && c.RefreshToken == refreshToken
                ),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task Refresh_OnSuccess_ShouldCallGetTokenStorageOnce()
    {
        // Arrange
        var request = new RefreshRequest(
            "eyJHBn01.",
            "M8Xy+-=="
        );

        // Act
        await _cut.Refresh(
            request
        );

        // Assert
        _mockTokenStorage
            .Received(1)
            .Get();
    }

    [Fact]
    public async Task Logout_OnSuccess_ShouldReturnOkResult()
    {
        // Arrange
        var request = new LogoutRequest(
            "M9XY-+1NXY=="
        );

        // Act
        var result = await _cut.Logout(
            request
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<OkResult>();
    }

    [Fact]
    public async Task Logout_OnSuccess_ShouldReturn200OkStatusCode()
    {
        // Arrange
        var request = new LogoutRequest(
            "M9XY-+1NXY=="
        );

        // Act
        var result = (OkResult)await _cut.Logout(
            request
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(200);
    }

    [Fact]
    public async Task Logout_WhenInvoked_ShouldCallHandleAsyncOnCommandHandlerOnce()
    {
        // Arrange
        var request = new LogoutRequest(
            "M9XY-+1NXY=="
        );

        // Act
        await _cut.Logout(
            request
        );

        // Assert
        await _mockLogout
            .Received(1)
            .HandleAsync(
                Arg.Any<LogoutCommand>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Theory]
    [InlineData("M9XY-+1NXY==")]
    [InlineData("M8Xy+-==")]
    public async Task Logout_WhenInvokedWithParameters_ShouldPassThemToCommand(
        string refreshToken
    )
    {
        // Arrange
        var request = new LogoutRequest(
            refreshToken
        );

        // Act
        await _cut.Logout(
            request
        );

        // Assert
        await _mockLogout
            .Received(1)
            .HandleAsync(
                Arg.Is<LogoutCommand>(
                    c => c.RefreshToken == refreshToken
                ),
                Arg.Any<CancellationToken>()
            );
    }
}