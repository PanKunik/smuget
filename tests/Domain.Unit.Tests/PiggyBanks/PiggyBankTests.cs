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
            Constants.PiggyBank.Goal
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
            Constants.PiggyBank.Goal
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
            null
        );

        // Act & Assert
        createPiggyBank
            .Should()
            .ThrowExactly<GoalIsNullException>();
    }

    [Fact]
    public void PiggyBank_WhenPassedWithGoalSetToFalseAndGoalGreaterThanZero_ShouldThrowXYZ()
    {
        // Arrange
        var createPiggyBank = () => new PiggyBank(
            Constants.PiggyBank.Id,
            Constants.PiggyBank.Name,
            false,
            new(1000m)
        );

        // Act & Assert
        createPiggyBank
            .Should()
            .ThrowExactly<InvalidPiggyBankGoalValueException>();
    }

    [Fact]
    public void PiggBank_WhenPassedWithGoalSetToTrueAndGoalLowerOrEqualToZero_ShouldThrowXYZ()
    {
        // Arrange
        var createPiggyBank = () => new PiggyBank(
            Constants.PiggyBank.Id,
            Constants.PiggyBank.Name,
            true,
            new(0m)
        );

        // Act & Assert
        createPiggyBank
            .Should()
            .ThrowExactly<InvalidPiggyBankGoalValueException>();
    }
}