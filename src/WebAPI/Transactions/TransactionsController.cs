using Application.Abstractions.CQRS;
using Application.PiggyBanks.AddTransaction;
using Application.PiggyBanks.RemoveTransaction;
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
    private readonly ICommandHandler<RemoveTransactionCommand> _removeTransaction;

    public TransactionsController(
        IUserService userService,
        ICommandHandler<AddTransactionCommand> addTransaction,
        ICommandHandler<RemoveTransactionCommand> removeTransaction
    )
    {
        _userService = userService;
        _addTransaction = addTransaction;
        _removeTransaction = removeTransaction;
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
            ),
            token
        );

        return Created("", null);
    }

    [HttpDelete("{transactionId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Remove(
        [FromRoute] Guid piggyBankId,
        [FromRoute] Guid transactionId,
        CancellationToken token = default
    )
    {
        await _removeTransaction.HandleAsync(
            new RemoveTransactionCommand(
                piggyBankId,
                transactionId,
                _userService.UserId
            ),
            token
        );

        return NoContent();
    }
}