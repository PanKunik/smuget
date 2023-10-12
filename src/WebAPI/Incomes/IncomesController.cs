using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddIncome;
using Application.MonthlyBillings.RemoveIncome;
using Application.MonthlyBillings.UpdateIncome;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.Incomes;

[Authorize]
[ApiController]
[Route("api/monthlyBillings/{monthlyBillingId}/incomes")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class IncomesController : ControllerBase
{
    private readonly ICommandHandler<AddIncomeCommand> _addIncome;
    private readonly ICommandHandler<UpdateIncomeCommand> _updateIncome;
    private readonly ICommandHandler<RemoveIncomeCommand> _removeIncome;

    public IncomesController(
        ICommandHandler<AddIncomeCommand> addIncome,
        ICommandHandler<UpdateIncomeCommand> updateIncome,
        ICommandHandler<RemoveIncomeCommand> removeIncome
    )
    {
        _addIncome = addIncome;
        _updateIncome = updateIncome;
        _removeIncome = removeIncome;
    }

    /// <summary>
    /// Adds income to a monthly billing specified by identifier.
    /// </summary>
    /// <param name="monthlyBillingId">Identifier of a monthly billing.</param>
    [HttpPost]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> AddIncome(
        [FromRoute(Name = "monthlyBillingId")] Guid monthlyBillingId,
        [FromBody] AddIncomeRequest request,
        CancellationToken token = default
    )
    {
        var (name, amount, currency, include) = request;

        await _addIncome.HandleAsync(
            new AddIncomeCommand(
                monthlyBillingId,
                name,
                amount,
                currency,
                include),
            token);

        return Created("", null);
    }

    /// <summary>
    /// Updates specified income in monthly billing.
    /// </summary>
    /// <param name="monthlyBillingId">Identifier of a monthly billing.</param>
    /// <param name="incomeId">Identifier of an income to update.</param>
    [HttpPut("{incomeId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> UpdateIncome(
        [FromRoute] Guid monthlyBillingId,
        [FromRoute] Guid incomeId,
        [FromBody] UpdateIncomeRequest request,
        CancellationToken token = default
    )
    {
        var (name, moneyAmount, currency, include) = request;

        await _updateIncome.HandleAsync(
            new UpdateIncomeCommand(
                monthlyBillingId,
                incomeId,
                name,
                moneyAmount,
                currency,
                include
            ),
            token
        );

        return NoContent();
    }

    /// <summary>
    /// Removes specified income from monthly billing by income identifier.
    /// </summary>
    /// <param name="monthlyBillingId">Identifier of a monthly billing.</param>
    /// <param name="incomeId">Identifier of an income to remove.</param>
    [HttpDelete("{incomeId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> RemoveIncome(
        [FromRoute] Guid monthlyBillingId,
        [FromRoute] Guid incomeId,
        CancellationToken token = default
    )
    {
        await _removeIncome.HandleAsync(
            new RemoveIncomeCommand(
                monthlyBillingId,
                incomeId
            ),
            token
        );

        return NoContent();
    }
}