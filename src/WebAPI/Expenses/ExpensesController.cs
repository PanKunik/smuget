using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddExpense;
using Application.MonthlyBillings.RemoveExpense;
using Application.MonthlyBillings.UpdateExpense;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Services.Users;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.Expenses;

[Authorize]
[ApiController]
[Route("api/monthlyBillings/{monthlyBillingId}/plans/{planId}/expenses")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class ExpensesController
    : ControllerBase
{
    private readonly ICommandHandler<AddExpenseCommand> _addExpense;
    private readonly ICommandHandler<UpdateExpenseCommand> _updateExpense;
    private readonly ICommandHandler<RemoveExpenseCommand> _removeExpense;
    private readonly IUserService _userService;

    public ExpensesController(
        ICommandHandler<AddExpenseCommand> addExpense,
        ICommandHandler<RemoveExpenseCommand> removeExpense,
        ICommandHandler<UpdateExpenseCommand> updateExpense,
        IUserService userService
    )
    {
        _addExpense = addExpense
            ?? throw new ArgumentNullException(nameof(addExpense));
        _updateExpense = updateExpense
            ?? throw new ArgumentNullException(nameof(updateExpense));
        _removeExpense = removeExpense
            ?? throw new ArgumentNullException(nameof(removeExpense));
        _userService = userService
            ?? throw new ArgumentNullException(nameof(userService));
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
                description,
                _userService.UserId
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
                description,
                _userService.UserId
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
                expenseId,
                _userService.UserId
            ),
            token
        );

        return NoContent();
    }
}