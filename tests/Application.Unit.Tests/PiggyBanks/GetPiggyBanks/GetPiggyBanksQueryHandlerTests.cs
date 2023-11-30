using Application.PiggyBanks;
using Application.PiggyBanks.GetPiggyBanks;
using Application.PiggyBanks.Mappings;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.PiggyBanks.GetPiggyBanks;

public sealed class GetPiggyBanksQueryHandlerTests
{
    private readonly IPiggyBanksRepository _mockRepository;

    private readonly GetPiggyBanksQueryHandler _cut;

    public GetPiggyBanksQueryHandlerTests()
    {
        _mockRepository = Substitute.For<IPiggyBanksRepository>();

        _mockRepository
            .GetAll(
                Arg.Is<UserId>(
                    i => i.Value == Constants.User.Id
                )
            )
            .Returns(
                new List<PiggyBank>()
                {
                    PiggyBanksUtilities.CreatePiggyBank(TransactionsUtilities.CreateTransactions(1).ToList())
                }
            );

        _cut = new GetPiggyBanksQueryHandler(
            _mockRepository
        );
    }

    [Fact]
    public async Task HandleAsync_WhenCalled_ShouldCallGetAllOnRepositoryOnce()
    {
        // Arrange
        var query = GetPiggyBanksQueryUtilities.CreateQuery();

        // Act
        await _cut.HandleAsync(
            query,
            default
        );

        // Assert
        await _mockRepository
            .Received(1)
            .GetAll(
                Arg.Is<UserId>(
                    i => i.Value == Constants.User.Id
                )
            );
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_ShouldReturnListOfExpectedObjects()
    {
        // Arrange
        var query = GetPiggyBanksQueryUtilities.CreateQuery();

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
                new List<PiggyBankDTO>()
                {
                    new PiggyBankDTO()
                    {
                        Id = Constants.PiggyBank.Id,
                        Name = Constants.PiggyBank.Name,
                        WithGoal = Constants.PiggyBank.WithGoal,
                        Goal = Constants.PiggyBank.Goal,
                        Transactions = new List<TransactionDTO>()
                        {
                            new TransactionDTO()
                            {
                                Id = Constants.Transaction.Id,
                                Value = Constants.Transaction.Value,
                                Date = Constants.Transaction.Date
                            }
                        }
                    }
                }
            );
    }

    [Fact]
    public async Task HandleAsync_WhenPiggyBanksWerentFond_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetPiggyBanksQuery(
            Guid.NewGuid()
        );

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
            .BeEquivalentTo(new List<PiggyBankDTO>());
    }
}