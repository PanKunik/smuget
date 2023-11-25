using Domain.Exceptions;
using Domain.PiggyBanks;
using Domain.Unit.Tests.PiggyBanks.TestUtilities;

namespace Domain.Unit.Tests.PiggyBanks;

public sealed class TransactionTests
{
    [Fact]
    public void Transaction_WhenCalledWithProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createTransaction = () => TransactionUtilities.CreateTransaction();

        // Act
        var result = createTransaction();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<Transaction>(
                t => t.Id == Constants.Transaction.Id
                  && t.Value == Constants.Transaction.Value
                  && t.Date == Constants.Transaction.Date
            );
    }

    [Fact]
    public void Transaction_WhenPassedNullTransactionId_ShouldThrowTransactionIdIsNullException()
    {
        // Arrange
        var createTransaction = () => new Transaction(
            null,
            Constants.Transaction.Value,
            Constants.Transaction.Date
        );

        // Act & Assert
        createTransaction
            .Should()
            .ThrowExactly<TransactionIdIsNullException>();
    }

    [Fact]
    public void Transaction_WhenPassedZeroAsValue_ShouldReturnTransactionValueEqualToZeroException()
    {
        // Arrange
        var createTransaction = () => new Transaction(
            Constants.Transaction.Id,
            0,
            Constants.Transaction.Date
        );

        // Act & Assert
        createTransaction
            .Should()
            .ThrowExactly<TransactionValueEqualToZeroException>();
    }
}