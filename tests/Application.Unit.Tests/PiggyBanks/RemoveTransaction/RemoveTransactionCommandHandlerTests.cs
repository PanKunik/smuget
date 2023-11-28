using System.Reflection.Metadata;
using Application.Exceptions;
using Application.PiggyBanks.RemoveTransaction;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.PiggyBanks.RemoveTransaction;

public sealed class RemoveTransactionCommandHandlerTests
{
    private readonly RemoveTransactionCommandHandler _cut;

    private readonly IPiggyBanksRepository _mockRepository;

    public RemoveTransactionCommandHandlerTests()
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
            .Returns(PiggyBanksUtilities.CreatePiggyBank());

        _cut = new RemoveTransactionCommandHandler(
            _mockRepository
        );
    }

    [Fact]
    public async Task HandleAsync_WhenCalled_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = RemoveTransactionCommandUtilities.CreateCommand();

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
                    i => i.Value == Constants.PiggyBank.Id
                ),
                Arg.Is<UserId>(
                    i => i.Value == Constants.User.Id
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenPiggyBankNotFound_ShouldThrowPiggyBankNotFoundException()
    {
        // Arrange
        var command = new RemoveTransactionCommand(
            Guid.NewGuid(),
            Constants.Transaction.Id,
            Constants.User.Id
        );

        var removeTransaction = async () => await _cut.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await removeTransaction
            .Should()
            .ThrowExactlyAsync<PiggyBankNotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        PiggyBank passedArgument = null;
        await _mockRepository
            .Save(
                Arg.Do<PiggyBank>(p => passedArgument = p)
            );

        var command = RemoveTransactionCommandUtilities.CreateCommand();

        // Act
        await _cut.HandleAsync(
            command,
            default
        );

        // Assert
        await _mockRepository
            .Received(1)
            .Save(Arg.Is(passedArgument));

        passedArgument?.Transactions
            .Should()
            .HaveCount(1);
    }
}