using Application.Exceptions;
using Application.PiggyBanks.CreatePiggyBank;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.PiggyBanks.CreatePiggyBank;

public sealed class CreatePiggyBankCommandHandlerTests
{
    private readonly CreatePiggyBankCommandHandler _cut;

    private readonly IPiggyBanksRepository _mockRepository;

    public CreatePiggyBankCommandHandlerTests()
    {
        _mockRepository = Substitute.For<IPiggyBanksRepository>();

        _mockRepository
            .GetByName(
                Arg.Is<Name>(
                    n => n.Value == Constants.PiggyBank.Name
                ),
                Arg.Is<UserId>(
                    u => u.Value == Constants.User.Id
                )
            )
            .Returns(PiggyBanksUtilities.CreatePiggyBank());

        _cut = new CreatePiggyBankCommandHandler(
            _mockRepository
        );
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_ShouldPass()
    {
        // Arrange
        var command = CreatePiggyBankCommandUtilities.CreateCommand();

        var handle = async () => await _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await handle
            .Should()
            .NotThrowAsync();
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByNameOnRepositoryOnce()
    {
        // Arrange
        var command = CreatePiggyBankCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            command,
            default
        );

        // Assert
        await _mockRepository
            .Received(1)
            .GetByName(
                Arg.Is<Name>(
                    n => n.Value == command.Name
                ),
                Arg.Is<UserId>(
                    u => u.Value == command.UserId
                )
            );
    }

    [Fact]
    public async void HandleAsync_WhenFoundPiggyBankWithSameName_ShouldThrow()
    {
        // Arrnage
        var command = new CreatePiggyBankCommand(
            Constants.PiggyBank.Name,
            Constants.PiggyBank.WithGoal,
            Constants.PiggyBank.Goal,
            Constants.User.Id
        );

        var handle = async () => await _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await handle
            .Should()
            .ThrowExactlyAsync<PiggyBankAlreadyExistsException>();
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var command = CreatePiggyBankCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            command,
            default
        );

        // Assert
        await _mockRepository
            .Received(1)
            .Save(Arg.Any<PiggyBank>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShoudlContainNewTransaction()
    {
        // Arrange
        PiggyBank passedArgument = null;

        await _mockRepository
            .Save(Arg.Do<PiggyBank>(p => passedArgument = p));

        var command = new CreatePiggyBankCommand(
            "New piggy bank 2",
            Constants.PiggyBank.WithGoal,
            Constants.PiggyBank.Goal,
            Constants.User.Id
        );

        // Act
        await _cut.HandleAsync(
            command,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.Name.Value
            .Should()
            .Be(command.Name);

        passedArgument?.WithGoal
            .Should()
            .Be(command.WithGoal);

        passedArgument?.Goal.Value
            .Should()
            .Be(command.Goal);

        passedArgument?.UserId.Value
            .Should()
            .Be(command.UserId);
    }
}