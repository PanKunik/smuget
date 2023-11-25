using Application.Abstractions.CQRS;
using Application.PiggyBanks.CreatePiggyBank;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Users;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.PiggyBanks;

[Authorize]
[ApiController]
[Route("api/piggy-banks")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class PiggyBanksController
    : ControllerBase
{
    private readonly ICommandHandler<CreatePiggyBankCommand> _createPiggyBank;
    private readonly IUserService _userService;

    public PiggyBanksController(
        ICommandHandler<CreatePiggyBankCommand> createPiggyBank,
        IUserService userService
    )
    {
        _createPiggyBank = createPiggyBank
            ?? throw new ArgumentNullException(nameof(createPiggyBank));
        _userService = userService
            ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpPost]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePiggyBankRequest request,
        CancellationToken token = default
    )
    {
        var (
            name,
            withGoal,
            goal
        ) = request;

        await _createPiggyBank.HandleAsync(
            new CreatePiggyBankCommand(
                name,
                withGoal,
                goal,
                _userService.UserId
            ),
            token
        );

        return Created("", null);
    }
}