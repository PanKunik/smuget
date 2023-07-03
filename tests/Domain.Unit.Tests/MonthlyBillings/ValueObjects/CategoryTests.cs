using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class CategoryTests
{
    [Fact]
    public void Category_WhenPassedValidData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createCategory = () => new Category("Fuel");

        // Act
        var result = createCategory();

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be("Fuel");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Category_WhenPassedNullOrWhiteSpaceCategory_ShouldThrowCategoryIsNullException(string value)
    {
        // Arrange
        var createCategory = () => new Category(value);

        // Act & Assert
        Assert.Throws<CategoryIsNullException>(createCategory);
    }
}