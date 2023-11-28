using System.Collections.Generic;
using Domain.Exceptions;
using Domain.PiggyBanks;
using Domain.Unit.Tests.PiggyBanks.TestUtilities;

namespace Domain.Unit.Tests.PiggyBanks;

public sealed class PiggyBankTests
{
    [Fact]
    public void PiggyBank_WhenCalled_ShouldReturnExpectedObject()
    {
        // Arrange
        var createPiggyBank = () => PiggyBankUtilities.CreatePiggyBank();

        // Act
        var result = createPiggyBank();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<PiggyBank>(
                p => p.Id == Constants.PiggyBank.Id
                  && p.Name == Constants.PiggyBank.Name
                  && p.WithGoal == Constants.PiggyBank.WithGoal
                  && p.Goal == Constants.PiggyBank.Goal
            );
    }

    [Fact]
    public void PiggyBank_WhenPassedNullPiggyBankId_ShouldThrowPiggyBankIdIsNullException()
    {
        // Arrange
        var createPiggyBank = () => new PiggyBank(
            null,
            Constants.PiggyBank.Name,
            Constants.PiggyBank.WithGoal,
            Constants.PiggyBank.Goal,
            Constants.PiggyBank.UserId
        );

        // Act & Assert
        createPiggyBank
            .Should()
            .ThrowExactly<PiggyBankIdIsNullException>();
    }

    [Fact]
    public void PiggyBank_WhenPassedNullName_ShouldThrowNameIsNullException()
    {
        // Arrange
        var createPiggyBank = () => new PiggyBank(
            Constants.PiggyBank.Id,
            null,
            Constants.PiggyBank.WithGoal,
            Constants.PiggyBank.Goal,
            Constants.PiggyBank.UserId
        );

        // Act & Assert
        createPiggyBank
            .Should()
            .ThrowExactly<NameIsNullException>();
    }

    [Fact]
    public void PiggyBank_WhenPassedNullGoal_ShouldThrowGoalIsNullException()
    {
        // Arrange
        var createPiggyBank = () => new PiggyBank(
            Constants.PiggyBank.Id,
            Constants.PiggyBank.Name,
            Constants.PiggyBank.WithGoal,
            null,
            Constants.PiggyBank.UserId
        );

        // Act & Assert
        createPiggyBank
            .Should()
            .ThrowExactly<GoalIsNullException>();
    }

    [Fact]
    public void PiggyBank_WhenPassedNullUserId_ShouldThrowUserIdIsNullException()
    {
        // Arrange
        var createPiggyBank = () => new PiggyBank(
            Constants.PiggyBank.Id,
            Constants.PiggyBank.Name,
            Constants.PiggyBank.WithGoal,
            Constants.PiggyBank.Goal,
            null
        );

        // Act & Assert
        createPiggyBank
            .Should()
            .ThrowExactly<UserIdIsNullException>();
    }

    [Fact]
    public void PiggyBank_WhenPassedWithGoalSetToFalseAndGoalGreaterThanZero_ShouldThrowXYZ()
    {
        // Arrange
        var createPiggyBank = () => new PiggyBank(
            Constants.PiggyBank.Id,
            Constants.PiggyBank.Name,
            false,
            new(1000m),
            Constants.PiggyBank.UserId
        );

        // Act & Assert
        createPiggyBank
            .Should()
            .ThrowExactly<InvalidPiggyBankGoalValueException>();
    }

    [Fact]
    public void PiggyBank_WhenPassedWithGoalSetToTrueAndGoalLowerOrEqualToZero_ShouldThrowXYZ()
    {
        // Arrange
        var createPiggyBank = () => new PiggyBank(
            Constants.PiggyBank.Id,
            Constants.PiggyBank.Name,
            true,
            new(0m),
            Constants.PiggyBank.UserId
        );

        // Act & Assert
        createPiggyBank
            .Should()
            .ThrowExactly<InvalidPiggyBankGoalValueException>();
    }

    [Fact]
    public void AddTransaction_WhenPassedProperData_ShouldAddTransactionToPiggyBank()
    {
        // Arrange
        var piggyBank = PiggyBankUtilities.CreatePiggyBank();
        var transaction = TransactionUtilities.CreateTransaction();

        // Act
        piggyBank.AddTransaction(transaction);

        // Assert
        piggyBank.Transactions
            .Should()
            .BeEquivalentTo(
                new List<Transaction>()
                {
                    new Transaction(
                        Constants.Transaction.Id,
                        Constants.Transaction.Value,
                        Constants.Transaction.Date
                    )
                }
            );
    }

    [Fact]
    public void AddTransaction_WhenPassedTransactionThatAlreadyExists_ShouldThrowTransactionAlreadyExists()
    {
        // Arrange
        var piggyBank = PiggyBankUtilities.CreatePiggyBank();
        var transaction = TransactionUtilities.CreateTransaction();
        piggyBank.AddTransaction(transaction);
        var addTransaction = () => piggyBank.AddTransaction(transaction);

        // Act & Assert
        addTransaction
            .Should()
            .ThrowExactly<TransactionAlreadyExistsException>();
    }

    [Fact]
    public void RemoveTransaction_WhenPassedProperData_ShouldRemoveTransactionFromPiggyBank()
    {
        // Arrange
        var piggyBank = PiggyBankUtilities.CreatePiggyBank(
            new List<Transaction>()
            {
                new Transaction(
                    Constants.Transaction.Id,
                    Constants.Transaction.Value,
                    Constants.Transaction.Date
                )
            }
        );

        // Act
        piggyBank.RemoveTransaction(Constants.Transaction.Id);

        // Assert
        piggyBank.Transactions
            .Should()
            .HaveCount(0);
    }

    [Fact]
    public void RemoveTransaction_WhenTransactionWasntFound_ShouldThrowTransactionNotFoundException()
    {
        // Arrange
        var piggyBank = PiggyBankUtilities.CreatePiggyBank();
        var removeTransaction = () => piggyBank.RemoveTransaction(Constants.Transaction.Id);

        // Act & Assert
        removeTransaction
            .Should()
            .ThrowExactly<TransactionNotFoundException>();
    }
}