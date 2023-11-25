using System;
using Domain.Exceptions;
using Domain.PiggyBanks;

namespace Domain.Unit.Tests.PiggyBanks.ValueObjects;

public sealed class TransactionIdTests
{
    [Fact]
    public void TransactionId_WhenCalledWithProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var createTransactionId = () => new TransactionId(guid);

        // Act
        var result = createTransactionId();

        // Assert
        result
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Be(guid);
    }

    [Fact]
    public void TransactionId_WhenPassedEmptyGuid_ShouldThrowInvalidTransactionIdException()
    {
        // Arrange
        var createTransactionId = () => new TransactionId(Guid.Empty);

        // Act & Assert
        createTransactionId
            .Should()
            .ThrowExactly<InvalidTransactionIdException>();
    }

    [Fact]
    public void Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var compareTo = new TransactionId(guid);
        var cut = new TransactionId(guid);

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Equals_WhenCalledForObjectWithOtherValue_ShouldReturnFalse()
    {
        // Arrange
        var compareTo = new TransactionId(Guid.NewGuid());
        var cut = new TransactionId(Guid.NewGuid());

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeFalse();
    }
}