using Application.Abstractions.CQRS;
using Application.MonthlyBillings.Commands.AddExpense;
using Application.MonthlyBillings.Commands.AddIncome;
using Application.MonthlyBillings.Commands.AddPlan;
using Application.MonthlyBillings.Commands.CloseMonthlyBilling;
using Application.MonthlyBillings.Commands.OpenMonthlyBilling;
using Application.MonthlyBillings.DTO;
using Application.MonthlyBillings.Queries.GetByYearAndMonth;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.MonthlyBillings;

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

    public MonthlyBillingsController(
        ICommandHandler<OpenMonthlyBillingCommand> openMonthlyBilling,
        ICommandHandler<AddIncomeCommand> addIncome,
        ICommandHandler<AddPlanCommand> addPlan,
        ICommandHandler<AddExpenseCommand> addExpense,
        IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO> getMonthlyBilling,
        ICommandHandler<CloseMonthlyBillingCommand> closeMonthlyBilling
    )
    {
        _openMonthlyBilling = openMonthlyBilling;
        _addIncome = addIncome;
        _addPlan = addPlan;
        _addExpense = addExpense;
        _getMonthlyBilling = getMonthlyBilling;
        _closeMonthlyBilling = closeMonthlyBilling;
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
}