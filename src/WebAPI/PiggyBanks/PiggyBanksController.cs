using Application.Abstractions.CQRS;
using Application.PiggyBanks;
using Application.PiggyBanks.CreatePiggyBank;
using Application.PiggyBanks.GetPiggyBankById;
using Application.PiggyBanks.GetPiggyBanks;
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
    private readonly IQueryHandler<GetPiggyBankByIdQuery, PiggyBankDTO> _getById;
    private readonly IQueryHandler<GetPiggyBanksQuery, IEnumerable<PiggyBankDTO?>> _getAll;
    private readonly IUserService _userService;

    public PiggyBanksController(
        ICommandHandler<CreatePiggyBankCommand> createPiggyBank,
        IQueryHandler<GetPiggyBankByIdQuery, PiggyBankDTO> getById,
        IQueryHandler<GetPiggyBanksQuery, IEnumerable<PiggyBankDTO?>> getAll,
        IUserService userService
    )
    {
        _createPiggyBank = createPiggyBank
            ?? throw new ArgumentNullException(nameof(createPiggyBank));
        _getById = getById
            ?? throw new ArgumentNullException(nameof(getById));
        _getAll = getAll
            ?? throw new ArgumentNullException(nameof(getAll));
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

    [HttpGet("{piggyBankId}")]
    [ProducesResponseType(typeof(PiggyBankDTO), Status200OK)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid piggyBankId,
        CancellationToken token = default
    )
    {
        var result = await _getById.HandleAsync(
            new GetPiggyBankByIdQuery(
                piggyBankId,
                _userService.UserId
            ),
            token
        );

        return Ok(result);
    }

    [HttpGet()]
    [ProducesResponseType(typeof(PiggyBankDTO), Status200OK)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        CancellationToken token = default
    )
    {
        var result = await _getAll.HandleAsync(
            new GetPiggyBanksQuery(
                _userService.UserId
            ),
            token
        );

        return Ok(result);
    }
}