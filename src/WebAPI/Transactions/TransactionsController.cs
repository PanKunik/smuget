using Application.Abstractions.CQRS;
using Application.PiggyBanks.AddTransaction;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Users;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.Transactions;

[Authorize]
[ApiController]
[Route("api/piggy-banks/{piggyBankId}/transactions")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class TransactionsController
    : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICommandHandler<AddTransactionCommand> _addTransaction;

    public TransactionsController(
        IUserService userService,
        ICommandHandler<AddTransactionCommand> addTransaction
    )
    {
        _userService = userService;
        _addTransaction = addTransaction;
    }

    [HttpPost]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Add(
        [FromRoute] Guid piggyBankId,
        [FromBody] AddTransactionRequest request,
        CancellationToken token = default
    )
    {
        var (
            value,
            date
        ) = request;

        await _addTransaction.HandleAsync(
            new AddTransactionCommand(
                piggyBankId,
                value,
                date,
                _userService.UserId
            )
        );

        return Created("", null);
    }
}