using System;
using System.Reflection.Metadata;
using Domain.Exceptions;
using Domain.MonthlyBillings;
using Domain.Unit.Tests.MonthlyBillings.TestUtilities;

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

    [Fact]
    public void Update_WhenPassedNullMoney_ShouldThrowMoneyIsNullException()
    {
        // Arrange
        var cut = new Expense(
            Constants.Expense.Id,
            Constants.Expense.Money,
            Constants.Expense.ExpenseDate,
            Constants.Expense.Descripiton
        );

        var updateExpense = () => cut.Update(
            null,
            new DateTimeOffset(new DateTime(2023, 9, 2, 15, 0, 9)),
            "Updated description of a expense"
        );

        // Act & Assert
        Assert.Throws<MoneyIsNullException>(updateExpense);
    }

    [Fact]
    public void Update_WhenPassedProperData_ShouldUpdateExpense()
    {
        // Arrange
        var cut = new Expense(
            Constants.Expense.Id,
            Constants.Expense.Money,
            Constants.Expense.ExpenseDate,
            Constants.Expense.Descripiton
        );

        // Act
        cut.Update(
            new Money(
                1234.56m,
                new Currency("USD")
            ),
            new DateTimeOffset(new DateTime(2023, 9, 2, 15, 0, 9)),
            "Updated description of a expense"
        );

        // Assert
        cut
            .Should()
            .Match<Expense>(
                e => e.Id == Constants.Expense.Id
                  && e.Money.Amount == 1234.56m
                  && e.Money.Currency.Value == "USD"
                  && e.ExpenseDate == new DateTimeOffset(new DateTime(2023, 9, 2, 15, 0, 9))
                  && e.Description == "Updated description of a expense"
            );
    }

    [Fact]
    public void Remove_WhenCalled_ShouldSetActiveToFalse()
    {
        // Arrange
        var cut = new Expense(
            Constants.Expense.Id,
            Constants.Expense.Money,
            Constants.Expense.ExpenseDate,
            Constants.Expense.Descripiton
        );

        // Act
        cut.Remove();

        // Assert
        cut.Active
            .Should()
            .BeFalse();
    }
}