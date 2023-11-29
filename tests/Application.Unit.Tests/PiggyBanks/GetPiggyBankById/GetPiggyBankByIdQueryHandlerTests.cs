using System.Security.Cryptography.X509Certificates;
using Application.Exceptions;
using Application.PiggyBanks;
using Application.PiggyBanks.GetPiggyBankById;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.PiggyBanks.GetPiggyBankById;

public sealed class GetPiggyBankByIdQueryHandlerTests
{
    private readonly GetPiggyBankByIdQueryHandler _cut;

    private readonly IPiggyBanksRepository _mockRepository;

    public GetPiggyBankByIdQueryHandlerTests()
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

        _cut = new GetPiggyBankByIdQueryHandler(
            _mockRepository
        );
    }
    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var query = GetPiggyBankByIdQueryUtilities.CreateQuery();

        // Act
        await _cut.HandleAsync(
            query,
            default
        );

        // Assert
        await _mockRepository
            .Received(1)
            .GetById(
                Arg.Is<PiggyBankId>(
                    pi => pi.Value == Constants.PiggyBank.Id
                ),
                Arg.Is<UserId>(
                    ui => ui.Value == Constants.User.Id
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenPiggyBankNotFound_ShouldThrowPiggyBankNotFoundException()
    {
        // Arrange
        var query = new GetPiggyBankByIdQuery(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        var getPiggyBankById = async () => await _cut.HandleAsync(
            query,
            default
        );

        // Act & Assert
        await getPiggyBankById
            .Should()
            .ThrowExactlyAsync<PiggyBankNotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_ShouldReturnExpectedObject()
    {
        // Arrange
        var query = GetPiggyBankByIdQueryUtilities.CreateQuery();

        // Act
        var result = await _cut.HandleAsync(
            query,
            default
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeEquivalentTo(
                new PiggyBankDTO()
                {
                    Id = Constants.PiggyBank.Id,
                    Name = Constants.PiggyBank.Name,
                    WithGoal = Constants.PiggyBank.WithGoal,
                    Goal = Constants.PiggyBank.Goal,
                    Transactions = TransactionsUtilities
                        .CreateTransactions()
                        .Select(
                            t => new TransactionDTO()
                            {
                                Id = t.Id.Value,
                                Value = t.Value,
                                Date = t.Date
                            }
                        )
                }
            );
    }
}