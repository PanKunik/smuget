using System;
using Domain.Exceptions;
using Domain.PiggyBanks;

namespace Domain.Unit.Tests.PiggyBanks.ValueObjects;

public sealed class PiggyBankIdTests
{
    [Fact]
    public void PiggyBankId_WhenPassedProperData_ShoudlReturnExpectedObject()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = new PiggyBankId(
            guid
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Be(guid);
    }

    [Fact]
    public void PiggyBankId_WhenPassedEmptyGuid_ShouldThrowInvalidPiggyBankIdException()
    {
        // Arrange
        var emptyGuid = Guid.Empty;
        var createPiggyBankId = () => new PiggyBankId(emptyGuid);

        // Act & Assert
        createPiggyBankId
            .Should()
            .ThrowExactly<InvalidPiggyBankIdException>();
    }

    [Fact]
    public void Equals_WhenPassedObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var compareTo = new PiggyBankId(guid);
        var cut = new PiggyBankId(guid);

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Equals_WhenPassedObjectWithOtherValue_ShouldReturnFalse()
    {
        // Arrange
        var compareTo = new PiggyBankId(Guid.NewGuid());
        var cut = new PiggyBankId(Guid.NewGuid());

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeFalse();
    }
}