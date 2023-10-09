using Domain.Exceptions;
using Domain.Users;

namespace Domain.Unit.Tests.Users.ValueObjects;

public sealed class PasswordTests
{
    [Fact]
    public void Password_OnSuccess_ShouldReturnExpectedObject()
    {
        // Arrange
        var value = "Passw0rf!2@.";

        // Act
        var result = new Password(
            value
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<Password>(
                p => p.Value == value
            );
    }

    [Theory]
    [InlineData("TREFDCXDEQ#$@%T#YEHTGDFVSQE#!$%^Y#%$H^TEBFSVEQR#!$%^Y#%^$JTRHEEFQR#!%^#YHTERGWEFQR#!$%^#Y%$HTEWR#!$%^#Y$THEGRQ#!$%^#YTHEGWFEQR#@^#Y%HTEGWQR#%@#Y$HTEGWFQR#%@^#Y$HTREWR#@$%#Y$THREGWER$^%#dhetsydhfgwtsgdt")]
    [InlineData("ajksdjkbajkdbajskbdkjabkjbckbjkskjbkjbckjasbjkxbcjcbjbd%^$JTRHEEFQR#!%^#YHTERGWEFQR#!$%^#Y%$HTEWR#!$%^#Y$THEGRQ#!$%^#YTHEGWFEQR#@^#Y%HTEGWQR#%@#Y$HTEGWFQR#%@^#Y$HTREWR#@$%#Y$THREGWER$^%#dhetsydhfgwtsgdt")]
    public void Password_WhenPassedValueIsTooLong_ShouldThrowPasswordIsTooLongException(string value)
    {
        // Arrange
        var createPassword = () => new Password(
            value
        );

        // Act & Assert
        Assert.Throws<PasswordIsTooLongException>(createPassword);
    }
}