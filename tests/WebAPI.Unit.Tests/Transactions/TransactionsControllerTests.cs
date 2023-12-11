using System;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.PiggyBanks.AddTransaction;
using Application.PiggyBanks.RemoveTransaction;
using Application.PiggyBanks.UpdateTransaction;
using Domain.PiggyBanks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebAPI.Services.Users;
using WebAPI.Transactions;

namespace WebAPI.Unit.Tests.Transactions;

public sealed class TransactionsControllerTests
{
    private readonly TransactionsController _cut;

    private readonly IUserService _mockUserService;
    private readonly ICommandHandler<AddTransactionCommand> _mockAddTransaction;
    private readonly ICommandHandler<UpdateTransactionCommand> _mockUpdateTransaction;
    private readonly ICommandHandler<RemoveTransactionCommand> _mockRemoveTransaction;

    public TransactionsControllerTests()
    {
        _mockUserService = Substitute.For<IUserService>();

        _mockAddTransaction = Substitute.For<ICommandHandler<AddTransactionCommand>>();
        _mockUpdateTransaction = Substitute.For<ICommandHandler<UpdateTransactionCommand>>();
        _mockRemoveTransaction = Substitute.For<ICommandHandler<RemoveTransactionCommand>>();

        _cut = new TransactionsController(
            _mockUserService,
            _mockAddTransaction,
            _mockUpdateTransaction,
            _mockRemoveTransaction
        );
    }

    [Fact]
    public async Task Add_OnSuccess_ShouldReturnCreatedResult()
    {
        // Arrange
        var request = new AddTransactionRequest(
            123.45m,
            new DateOnly(2020, 1, 1)
        );

        // Act
        var result = await _cut.Add(
            Guid.NewGuid(),
            request,
            default
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task Add_OnSuccess_ShouldReturnStatusCode201Created()
    {
        // Arrange
        var request = new AddTransactionRequest(
            123.45m,
            new DateOnly(2020, 1, 1)
        );

        // Act
        var result = (CreatedResult)await _cut.Add(
            Guid.NewGuid(),
            request,
            default
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(201);
    }

    [Fact]
    public async Task Add_WhenInvoked_ShouldCallCommandHandlerHandleAsyncOnce()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var request = new AddTransactionRequest(
            123.45m,
            new DateOnly(2020, 1, 1)
        );

        // Act
        await _cut.Add(
            guid,
            request,
            default
        );

        // Assert
        await _mockAddTransaction
            .Received(1)
            .HandleAsync(
                Arg.Is<AddTransactionCommand>(
                    c => c.PiggyBankId == guid
                      && c.Value == request.Value
                      && c.Date == request.Date
                ),
                default
            );
    }

    [Fact]
    public async Task Update_OnSuccess_ShouldReturnNoContentResult()
    {
        // Arrange
        var request = new UpdateTransactionRequest(
            21m,
            new DateOnly(2021, 12, 21)
        );

        // Act
        var result = await _cut.Update(
            Guid.NewGuid(),
            Guid.NewGuid(),
            request
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_OnSuccess_ShouldReturnStatusCode204NoContent()
    {
        // Arrange
        var request = new UpdateTransactionRequest(
            21m,
            new DateOnly(2021, 12, 21)
        );

        // Act
        var result = (NoContentResult)(await _cut.Update(
            Guid.NewGuid(),
            Guid.NewGuid(),
            request
        ));

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task Update_WhenInvoked_ShouldCallUpdateTransactionCommandHandler()
    {
        // Arrange
        var piggyBankId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var request = new UpdateTransactionRequest(
            123.45m,
            new DateOnly(2020, 3, 8)
        );

        // Act
        await _cut.Update(
            piggyBankId,
            transactionId,
            request,
            default
        );

        // Assert
        await _mockUpdateTransaction
            .Received(1)
            .HandleAsync(
                Arg.Is<UpdateTransactionCommand>(
                    c => c.PiggyBankId == piggyBankId
                      && c.TransactionId == transactionId
                      && c.Value == 123.45m
                      && c.Date == new DateOnly(2020, 3, 8)
                )
            );
    }

    [Fact]
    public async Task Remove_OnSuccess_ShouldReturnNoContentResult()
    {
        // Act
        var result = await _cut.Remove(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Remove_OnSuccess_ShouldReturnStatusCode204NoContent()
    {
        // Act
        var result = (NoContentResult)await _cut.Remove(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task Remove_WhenInvoked_ShouldCallHandleAsyncOnCommandHandler()
    {
        // Arrange
        var piggyBankId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();

        // Act
        await _cut.Remove(
            piggyBankId,
            transactionId
        );

        // Assert
        await _mockRemoveTransaction
            .Received(1)
            .HandleAsync(
                Arg.Is<RemoveTransactionCommand>(
                    c => c.PiggyBankId == piggyBankId
                      && c.TransactionId == transactionId
                )
            );
    }
}