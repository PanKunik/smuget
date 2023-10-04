using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddExpense;
using Application.MonthlyBillings.RemoveExpense;
using Application.MonthlyBillings.UpdateExpense;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.Expenses;

[ApiController]
[Route("api/monthlyBillings/{monthlyBillingId}/plans/{planId}/expenses")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class ExpensesController : ControllerBase
{
    private readonly ICommandHandler<AddExpenseCommand> _addExpense;
    private readonly ICommandHandler<UpdateExpenseCommand> _updateExpense;
    private readonly ICommandHandler<RemoveExpenseCommand> _removeExpense;

    public ExpensesController(
        ICommandHandler<AddExpenseCommand> addExpense,
        ICommandHandler<RemoveExpenseCommand> removeExpense,
        ICommandHandler<UpdateExpenseCommand> updateExpense
    )
    {
        _addExpense = addExpense;
        _updateExpense = updateExpense;
        _removeExpense = removeExpense;
    }

    /// <summary>
    /// Adds expense to a plan specified by id and monthly billing id.
    /// </summary>
    /// <param name="monthlyBillingId">Identifier of a monthly billing.</param>
    /// <param name="planId">Identifier of a plan.</param>
    [HttpPost]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> AddExpense(
        [FromRoute] Guid monthlyBillingId,
        [FromRoute] Guid planId,
        [FromBody, SwaggerRequestBody("Data about expense.")] AddExpenseRequest request,
        CancellationToken token = default
    )
    {
        var (amount, currency, date, description) = request;

        await _addExpense.HandleAsync(
            new AddExpenseCommand(
                monthlyBillingId,
                planId,
                amount,
                currency,
                date,
                description
            ),
            token
        );

        return Created("", null);
    }

    /// <summary>
    /// Updates specified expense in monthly billing.
    /// </summary>
    /// <param name="monthlyBillingId">Identifier for a monthly billing.</param>
    /// <param name="planId">Identifier for a plan in monthly billing conatining the expense to update.</param>
    /// <param name="expenseId">Idenifier for a expense to update.</param>
    [HttpPut("{expenseId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> UpdateExpense(
        [FromRoute] Guid monthlyBillingId,
        [FromRoute] Guid planId,
        [FromRoute] Guid expenseId,
        [FromBody] UpdateExpenseRequest request,
        CancellationToken token = default
    )
    {
        var (moneyAmount, currency, expenseDate, description) = request;

        await _updateExpense.HandleAsync(
            new UpdateExpenseCommand(
                monthlyBillingId,
                planId,
                expenseId,
                moneyAmount,
                currency,
                expenseDate,
                description
            ),
            token
        );

        return NoContent();
    }

    /// <summary>
    /// Removes specified expense in plan in monthly billing.
    /// </summary>
    /// <param name="monthlyBillingId">Identifier for a monthly billing.</param>
    /// <param name="planId">Identifier for a plan in monthly billing conatining the expense to remove.</param>
    /// <param name="expenseId">Idenifier for a expense to remove.</param>
    [HttpDelete("{expenseId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> RemoveExpense(
        [FromRoute] Guid monthlyBillingId,
        [FromRoute] Guid planId,
        [FromRoute] Guid expenseId,
        CancellationToken token = default
    )
    {
        await _removeExpense.HandleAsync(
            new RemoveExpenseCommand(
                monthlyBillingId,
                planId,
                expenseId
            ),
            token
        );

        return NoContent();
    }
}