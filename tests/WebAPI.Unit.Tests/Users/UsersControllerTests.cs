using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.Users.Register;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Users;

namespace WebAPI.Unit.Tests.Users;

public sealed class UsersControllerTests
{
    private readonly UsersController _cut;

    private readonly Mock<ICommandHandler<RegisterCommand>> _mockRegister;

    public UsersControllerTests()
    {
        _mockRegister = new Mock<ICommandHandler<RegisterCommand>>();

        _cut = new UsersController(
            _mockRegister.Object
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
}