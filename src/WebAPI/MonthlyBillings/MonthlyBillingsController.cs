using Application.Abstractions.CQRS;
using Application.MonthlyBillings.CloseMonthlyBilling;
using Application.MonthlyBillings.OpenMonthlyBilling;
using Application.MonthlyBillings.ReopenMonthlyBilling;
using Application.MonthlyBillings;
using Application.MonthlyBillings.GetByYearAndMonth;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Services.Users;

namespace WebAPI.MonthlyBillings;

[Authorize]
[ApiController]
[Route("api/monthlyBillings")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class MonthlyBillingsController : ControllerBase
{
    private readonly ICommandHandler<OpenMonthlyBillingCommand> _openMonthlyBilling;
    private readonly IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO> _getMonthlyBilling;
    private readonly ICommandHandler<CloseMonthlyBillingCommand> _closeMonthlyBilling;
    private readonly ICommandHandler<ReopenMonthlyBillingCommand> _reopenMonthlyBilling;
    private readonly IUserService _userService;

    public MonthlyBillingsController(
        ICommandHandler<OpenMonthlyBillingCommand> openMonthlyBilling,
        IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO> getMonthlyBilling,
        ICommandHandler<CloseMonthlyBillingCommand> closeMonthlyBilling,
        ICommandHandler<ReopenMonthlyBillingCommand> reopenMonthlyBilling,
        IUserService userService
    )
    {
        _openMonthlyBilling = openMonthlyBilling;
        _getMonthlyBilling = getMonthlyBilling;
        _closeMonthlyBilling = closeMonthlyBilling;
        _reopenMonthlyBilling = reopenMonthlyBilling;
        _userService = userService;
    }

    /// <summary>
    /// Creates (opens) new monthly billing for specified month in a year.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Open(
        [FromBody] OpenMonthlyBillingRequest request,
        CancellationToken token = default)
    {
        var (year, month, currency) = request;

        await _openMonthlyBilling.HandleAsync(
            new OpenMonthlyBillingCommand(
                year,
                month,
                currency,
                _userService.UserId
            ),
            token
        );

        return Created("", null);
    }

    /// <summary>
    /// Gets monthly billing with all child object (plans, incomes and expenses) specified by year and month.
    /// </summary>
    /// <param name="year">Year of the monthly billing</param>
    /// <param name="month">Month of the monthly billing</param>
    [HttpGet("{year:int}/{month:int}")]
    [ProducesResponseType(typeof(MonthlyBillingDTO), Status200OK)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Get(
        [FromRoute] ushort year,
        [FromRoute] byte month,
        CancellationToken token = default
    )
    {
        var result = await _getMonthlyBilling.HandleAsync(
            new GetMonthlyBillingByYearAndMonthQuery(
                year,
                month,
                _userService.UserId
            ),
            token
        );

        return Ok(result);
    }

    /// <summary>
    /// Closes monthly billing for specified month in a year.
    /// </summary>
    /// <param name="year">Year of the monthly billing.</param>
    /// <param name="month">Month of the monthly billing.</param>
    [HttpPut("{year:int}/{month:int}/close")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Close(
        [FromRoute] ushort year,
        [FromRoute] byte month,
        CancellationToken token = default
    )
    {
        await _closeMonthlyBilling.HandleAsync(
            new CloseMonthlyBillingCommand(
                year,
                month,
                _userService.UserId
            ),
            token
        );

        return NoContent();
    }

    /// <summary>
    /// Reopens monthly billing for specified month in a year.
    /// </summary>
    /// <param name="year">Year of the monthly billing.</param>
    /// <param name="month">Month of the monthly billing.</param>
    [HttpPut("{year:int}/{month:int}/reopen")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Reopen(
        [FromRoute] ushort year,
        [FromRoute] byte month,
        CancellationToken token = default
    )
    {
        await _reopenMonthlyBilling.HandleAsync(
            new ReopenMonthlyBillingCommand(
                year,
                month,
                _userService.UserId
            ),
            token
        );

        return NoContent();
    }
}