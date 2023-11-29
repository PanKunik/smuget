using System;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.PiggyBanks;
using Application.PiggyBanks.CreatePiggyBank;
using Application.PiggyBanks.GetPiggyBankById;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebAPI.PiggyBanks;
using WebAPI.Services.Users;

namespace WebAPI.Unit.Tests.PiggyBanks;

public sealed class PiggyBanksControllerTests
{
    private readonly PiggyBanksController _cut;

    private readonly ICommandHandler<CreatePiggyBankCommand> _mockCreateaPiggyBank;
    private readonly IQueryHandler<GetPiggyBankByIdQuery, PiggyBankDTO> _mockGetById;
    private readonly IUserService _mockUserService;

    public PiggyBanksControllerTests()
    {
        _mockCreateaPiggyBank = Substitute.For<ICommandHandler<CreatePiggyBankCommand>>();
        _mockGetById = Substitute.For<IQueryHandler<GetPiggyBankByIdQuery, PiggyBankDTO>>();
        _mockUserService = Substitute.For<IUserService>();

        _cut = new PiggyBanksController(
            _mockCreateaPiggyBank,
            _mockGetById,
            _mockUserService
        );
    }

    [Fact]
    public async Task Create_OnSuccess_ShouldReturnCreatedResult()
    {
        // Act
        var result = await _cut.Create(
            new(
                "Piggy bank",
                false,
                0m
            ),
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
    public async Task Create_OnSuccess_ShouldReturnStatusCode201Created()
    {
        // Act
        var result = (CreatedResult)await _cut.Create(
            new(
                "Piggy bank",
                false,
                0m
            ),
            default
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(201);
    }


    [Theory]
    [InlineData("New piggy bank", false, 0)]
    [InlineData("New piggy bank 2", true, 1500.23)]
    [InlineData("Piggy", true, 0)]
    public async Task Create_WhenInvoked_ShouldCallCreatePiggyBankCommandHandler(
        string name,
        bool withGoal,
        decimal goal
    )
    {
        // Act
        await _cut.Create(
            new(
                name,
                withGoal,
                goal
            )
        );

        // Assert
        await _mockCreateaPiggyBank
            .Received(1)
            .HandleAsync(
                Arg.Is<CreatePiggyBankCommand>(
                    c => c.Name == name
                      && c.WithGoal == withGoal
                      && c.Goal == goal
                )
            );
    }

    [Fact]
    public async Task Get_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Act
        var result = await _cut.Get(
            Guid.NewGuid(),
            default
        );

        // Assert
        result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Get_OnSuccess_ShouldReturnStatusCode200OK()
    {
        // Act
        var result = (OkObjectResult)await _cut.Get(
            Guid.NewGuid(),
            default
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(200);
    }

    [Fact]
    public async Task Get_WhenInvoked_ShouldCallHandleAsyncOnQueryHandler()
    {
        var piggyBankId = Guid.NewGuid();
        // Act
        await _cut.Get(
            piggyBankId,
            default
        );

        // Assert
        await _mockGetById
            .Received(1)
            .HandleAsync(
                Arg.Is<GetPiggyBankByIdQuery>(
                    q => q.PiggyBankId == piggyBankId
                )
            );
    }
}