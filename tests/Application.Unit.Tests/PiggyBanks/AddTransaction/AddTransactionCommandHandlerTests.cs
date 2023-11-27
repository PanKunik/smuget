using Application.Exceptions;
using Application.PiggyBanks.AddTransaction;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.PiggyBanks.AddTransaction;

public sealed class AddTransactionCommandHandlerTests
{
    private readonly AddTransactionCommandHandler _cut;

    private readonly IPiggyBanksRepository _mockRepository;

    public AddTransactionCommandHandlerTests()
    {
        _mockRepository = Substitute.For<IPiggyBanksRepository>();

        _mockRepository
            .GetById(
                Arg.Is<PiggyBankId>(
                    i => i.Value == Constants.PiggyBank.Id
                ),
                Arg.Is<UserId>(
                    i => i.Value == Constants.User.Id
                )
            )
            .Returns(
                PiggyBanksUtilities.CreatePiggyBank(
                    new List<Transaction>()
                )
            );

        _cut = new AddTransactionCommandHandler(
            _mockRepository
        );
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = AddTransactionCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            command,
            default
        );

        // Assert
        await _mockRepository
            .Received(1)
            .GetById(
                Arg.Is<PiggyBankId>(
                    i => i.Value == command.PiggyBankId
                ),
                Arg.Is<UserId>(
                    i => i.Value == command.UserId
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenPiggyBankWasntFound_ShouldThrowPiggyBankNotFoundException()
    {
        // Arrange
        var command = new AddTransactionCommand(
            Guid.NewGuid(),
            120m,
            new DateOnly(2020, 1, 1),
            Constants.User.Id
        );

        var handle = async () => await _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await handle
            .Should()
            .ThrowExactlyAsync<PiggyBankNotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var command = AddTransactionCommandUtilities.CreateCommand();

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

        var command = new AddTransactionCommand(
            Constants.PiggyBank.Id,
            -254.23m,
            new DateOnly(2022, 12, 12),
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

        passedArgument?.Transactions
            .Should()
            .HaveCount(1);
    }
}