using System;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.PiggyBanks.AddTransaction;
using Application.PiggyBanks.RemoveTransaction;
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
    private readonly ICommandHandler<RemoveTransactionCommand> _mockRemoveTransaction;

    public TransactionsControllerTests()
    {
        _mockUserService = Substitute.For<IUserService>();

        _mockAddTransaction = Substitute.For<ICommandHandler<AddTransactionCommand>>();
        _mockRemoveTransaction = Substitute.For<ICommandHandler<RemoveTransactionCommand>>();

        _cut = new TransactionsController(
            _mockUserService,
            _mockAddTransaction,
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