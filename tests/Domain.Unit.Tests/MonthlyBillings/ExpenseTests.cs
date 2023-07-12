using System;
using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings;

public sealed class ExpenseTests
{
    [Fact]
    public void Expense_WhenPassedValidParameters_ShouldReturnExpectedObject()
    {
        // Arrange
        var createExpense = () => new Expense(
            new ExpenseId(Guid.NewGuid()),
            new Money(172.04M, new Currency("EUR")),
            new DateTimeOffset(new DateTime(2020, 9, 21)),
            "Breaks"
        );

        // Act
        var result = createExpense();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<Expense>(
                e => e.Description == "Breaks"
                && e.ExpenseDate == new DateTimeOffset(new DateTime(2020, 9, 21))
                && e.Money == new Money(172.04M, new Currency("EUR"))
            );
    }

    [Fact]
    public void Expense_WhenPassedNullExpenseId_ShouldThrowExpenseIdIsNullException()
    {
        // Arrange
        var createExpense = () => new Expense(
            null,
            new Money(
                123.32M,
                new Currency("USD")
            ),
            new DateTimeOffset(),
            "TEST DESCRIPTION"
        );

        // Act & Assert
        Assert.Throws<ExpenseIdIsNullException>(createExpense);
    }

    [Fact]
    public void Expense_WhenPassedNullMoney_ShouldThrowMoneyIsNullException()
    {
        // Arrange
        var createExpense = () => new Expense(
            new ExpenseId(Guid.NewGuid()),
            null,
            new DateTimeOffset(new DateTime(2023, 1, 1)),
            "Marketplace"
        );

        // Act & Assert
        Assert.Throws<MoneyIsNullException>(createExpense);
    }
}