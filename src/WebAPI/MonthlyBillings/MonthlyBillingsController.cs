using Application.Abstractions.CQRS;
using Application.MonthlyBillings.Commands.AddExpense;
using Application.MonthlyBillings.Commands.AddIncome;
using Application.MonthlyBillings.Commands.AddPlan;
using Application.MonthlyBillings.Commands.CloseMonthlyBilling;
using Application.MonthlyBillings.Commands.OpenMonthlyBilling;
using Application.MonthlyBillings.Commands.RemoveIncome;
using Application.MonthlyBillings.Commands.RemovePlan;
using Application.MonthlyBillings.Commands.ReopenMonthlyBilling;
using Application.MonthlyBillings.Commands.UpdateIncome;
using Application.MonthlyBillings.Commands.UpdatePlan;
using Application.MonthlyBillings.DTO;
using Application.MonthlyBillings.Queries.GetByYearAndMonth;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.MonthlyBillings;

//TODO: Extract separate controllers for incomes/plans/expenses
[ApiController]
[Route("api/monthlyBillings")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class MonthlyBillingsController : ControllerBase
{
    private readonly ICommandHandler<OpenMonthlyBillingCommand> _openMonthlyBilling;
    private readonly ICommandHandler<AddIncomeCommand> _addIncome;
    private readonly ICommandHandler<AddPlanCommand> _addPlan;
    private readonly ICommandHandler<AddExpenseCommand> _addExpense;
    private readonly IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO> _getMonthlyBilling;
    private readonly ICommandHandler<CloseMonthlyBillingCommand> _closeMonthlyBilling;
    private readonly ICommandHandler<ReopenMonthlyBillingCommand> _reopenMonthlyBilling;
    private readonly ICommandHandler<UpdateIncomeCommand> _updateIncome;
    private readonly ICommandHandler<RemoveIncomeCommand> _removeIncome;
    private readonly ICommandHandler<RemovePlanCommand> _removePlan;
    private readonly ICommandHandler<UpdatePlanCommand> _updatePlan;

    public MonthlyBillingsController(
        ICommandHandler<OpenMonthlyBillingCommand> openMonthlyBilling,
        ICommandHandler<AddIncomeCommand> addIncome,
        ICommandHandler<AddPlanCommand> addPlan,
        ICommandHandler<AddExpenseCommand> addExpense,
        IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO> getMonthlyBilling,
        ICommandHandler<CloseMonthlyBillingCommand> closeMonthlyBilling,
        ICommandHandler<ReopenMonthlyBillingCommand> reopenMonthlyBilling,
        ICommandHandler<UpdateIncomeCommand> updateIncome,
        ICommandHandler<RemoveIncomeCommand> removeIncome,
        ICommandHandler<RemovePlanCommand> removePlan,
        ICommandHandler<UpdatePlanCommand> updatePlan
    )
    {
        _openMonthlyBilling = openMonthlyBilling;
        _addIncome = addIncome;
        _addPlan = addPlan;
        _addExpense = addExpense;
        _getMonthlyBilling = getMonthlyBilling;
        _closeMonthlyBilling = closeMonthlyBilling;
        _reopenMonthlyBilling = reopenMonthlyBilling;
        _updateIncome = updateIncome;
        _removeIncome = removeIncome;
        _removePlan = removePlan;
        _updatePlan = updatePlan;
    }

    /// <summary>
    /// Creates (opens) new monthly billing for specified month in a year.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Open(
        OpenMonthlyBillingRequest request,
        CancellationToken token = default)
    {
        var (year, month, currency) = request;

        await _openMonthlyBilling.HandleAsync(
            new OpenMonthlyBillingCommand(
                year,
                month,
                currency
            ),
            token
        );

        return Created("", null);
    }

    /// <summary>
    /// Adds income to a monthly billing specified by id.
    /// </summary>
    /// <param name="monthlyBillingId">Id of a monthly billing.</param>
    [HttpPost("{id}/incomes")]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> AddIncome(
        [FromRoute(Name = "id")] Guid monthlyBillingId,
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
    /// Adds plan to a monthly billing specified by id.
    /// </summary>
    /// <param name="monthlyBillingId">Id of a monthly billing.</param>
    [HttpPost("{id}/plans")]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> AddPlan(
        [FromRoute(Name = "id")] Guid monthlyBillingId,
        [FromBody] AddPlanRequest request,
        CancellationToken token = default
    )
    {
        var (category, amount, currency, sortOrder) = request;

        await _addPlan.HandleAsync(
            new AddPlanCommand(
                monthlyBillingId,
                category,
                amount,
                currency,
                sortOrder),
            token);

        return Created("", null);
    }

    /// <summary>
    /// Adds expense to a plan specified by id and monthly billing id.
    /// </summary>
    /// <param name="monthlyBillingId">Id of a monthly billing.</param>
    /// <param name="planId">Id of a plan.</param>
    [HttpPost("{id}/plans/{planId}/expenses")]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> AddExpense(
        [FromRoute(Name = "id")] Guid monthlyBillingId,
        [FromRoute(Name = "planId")] Guid planId,
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
    /// Gets monthly billing with all child object (plans, incomes and expenses) specified by year and month.
    /// </summary>
    [HttpGet("{year:int}/{month:int}")]
    [ProducesResponseType(typeof(MonthlyBillingDTO), Status200OK)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Get(
        [FromRoute] GetMonthlyBillingRequest request,
        CancellationToken token = default
    )
    {
        var (year, month) = request;

        var result = await _getMonthlyBilling.HandleAsync(
            new GetMonthlyBillingByYearAndMonthQuery(
                year,
                month
            ),
            token
        );

        return Ok(result);
    }

    /// <summary>
    /// Closes monthly billing for specified month in a year.
    /// </summary>
    [HttpPut("{year:int}/{month:int}/close")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Close(
        [FromRoute] CloseMonthlyBillingRequest request,
        CancellationToken token = default
    )
    {
        var (year, month) = request;

        await _closeMonthlyBilling.HandleAsync(
            new CloseMonthlyBillingCommand(
                year,
                month
            ),
            token
        );

        return NoContent();
    }

    /// <summary>
    /// Reopens monthly billing for specified month in a year.
    /// </summary>
    [HttpPut("{year:int}/{month:int}/reopen")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Reopen(
        [FromRoute] ReopenMonthlyBillingRequest request,
        CancellationToken token = default
    )
    {
        var (year, month) = request;

        await _reopenMonthlyBilling.HandleAsync(
            new ReopenMonthlyBillingCommand(
                year,
                month
            ),
            token
        );

        return NoContent();
    }

    /// <summary>
    /// Updates specified income in monthly billing.
    /// </summary>
    [HttpPut("{monthlyBillingId}/incomes/{incomeId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> UpdateIncome(
        [FromRoute(Name = "monthlyBillingId")] Guid monthlyBillingId,
        [FromRoute(Name = "incomeId")] Guid incomeId,
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
    /// Removes specified income from monthly billing by income id.
    /// </summary>
    [HttpDelete("{monthlyBillingId}/incomes/{incomeId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> RemoveIncome(
        [FromRoute] RemoveIncomeRequest request,
        CancellationToken token = default
    )
    {
        var (monthlyBillingId, incomeId) = request;

        await _removeIncome.HandleAsync(
            new RemoveIncomeCommand(
                monthlyBillingId,
                incomeId
            ),
            token
        );

        return NoContent();
    }

    /// <summary>
    /// Removes specified plan from monthly billing by plan id.
    /// </summary>
    [HttpDelete("{monthlyBillingId}/plans/{planId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> RemovePlan(
        [FromRoute] RemovePlanRequest request,
        CancellationToken token = default
    )
    {
        var (monthlyBillingId, planId) = request;

        await _removePlan.HandleAsync(
            new RemovePlanCommand(
                monthlyBillingId,
                planId
            ),
            token
        );

        return NoContent();
    }

    /// <summary>
    /// Updates specified plan in monthly billing.
    /// </summary>
    [HttpPut("{monthlyBillingId}/plans/{planId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> UpdatePlan(
        [FromRoute(Name = "monthlyBillingId")] Guid monthlyBillingId,
        [FromRoute(Name = "planId")] Guid planId,
        [FromBody] UpdatePlanRequest request,
        CancellationToken token = default
    )
    {
        var (category, moneyAmount, currency, sortOrder) = request;

        await _updatePlan.HandleAsync(
            new UpdatePlanCommand(
                monthlyBillingId,
                planId,
                category,
                moneyAmount,
                currency,
                sortOrder
            ),
            token
        );

        return NoContent();
    }
}