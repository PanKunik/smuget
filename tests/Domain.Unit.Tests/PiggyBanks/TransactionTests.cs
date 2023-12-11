using Domain.Exceptions;
using Domain.PiggyBanks;
using Domain.Unit.Tests.PiggyBanks.TestUtilities;
using System;

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
                  && t.Active == Constants.Transaction.Active
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

    [Fact]
    public void Remove_WhenCalled_ShouldChangeActiveToFalse()
    {
        // Arrange
        var transaction = TransactionUtilities.CreateTransaction();

        // Act
        transaction.Remove();

        // Assert
        transaction.Active
            .Should()
            .BeFalse();
    }

    [Fact]
    public void Update_OnSuccessShouldUpdateTransaction()
    {
        // Arrange
        var transaction = TransactionUtilities.CreateTransaction();

        // Act
        transaction.Update(
            15.01m,
            new DateOnly(2023, 1, 29)
        );

        // Assert
        transaction
            .Should()
            .Match<Transaction>(
                t => t.Value == 15.01m
                  && t.Date == new DateOnly(2023, 1, 29)
            );
    }

    [Fact]
    public void Update_WhenPassedValueEqualToZer_ShouldThrowTransactionValueEqualToZeroException()
    {
        // Arrange
        var transaction = TransactionUtilities.CreateTransaction();

        var updateTransaction = () => transaction.Update(
            0m,
            new DateOnly(2023, 1, 29)
        );

        // Act & Assert
        updateTransaction
            .Should()
            .ThrowExactly<TransactionValueEqualToZeroException>();
    }
}