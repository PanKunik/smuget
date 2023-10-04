using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddPlan;
using Application.MonthlyBillings.RemovePlan;
using Application.MonthlyBillings.UpdatePlan;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.Plans;

[ApiController]
[Route("api/monthlyBillings/{monthlyBillingId}/plans")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class PlansController : ControllerBase
{
    private readonly ICommandHandler<AddPlanCommand> _addPlan;
    private readonly ICommandHandler<UpdatePlanCommand> _updatePlan;
    private readonly ICommandHandler<RemovePlanCommand> _removePlan;

    public PlansController(
        ICommandHandler<AddPlanCommand> addPlan,
        ICommandHandler<UpdatePlanCommand> updatePlan,
        ICommandHandler<RemovePlanCommand> removePlan
    )
    {
        _addPlan = addPlan;
        _updatePlan = updatePlan;
        _removePlan = removePlan;
    }

    /// <summary>
    /// Adds plan to a monthly billing specified by identifier.
    /// </summary>
    /// <param name="monthlyBillingId">Identifier of a monthly billing.</param>
    [HttpPost]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> AddPlan(
        [FromRoute] Guid monthlyBillingId,
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
    /// Updates specified plan in monthly billing.
    /// </summary>
    /// <param name="monthlyBillingId">Identifier of a monthly billing.</param>
    /// <param name="planId">Identifier of a plan to update.</param>
    [HttpPut("{planId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> UpdatePlan(
        [FromRoute] Guid monthlyBillingId,
        [FromRoute] Guid planId,
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

    /// <summary>
    /// Removes specified plan from monthly billing by plan identifier.
    /// </summary>
    /// <param name="monthlyBillingId">Identifier of a monthly billing.</param>
    /// <param name="planId">Identifier of a plan to remove.</param>
    [HttpDelete("{planId}")]
    [ProducesResponseType(typeof(NoContentResult), Status204NoContent)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> RemovePlan(
        [FromRoute] Guid monthlyBillingId,
        [FromRoute] Guid planId,
        CancellationToken token = default
    )
    {
        await _removePlan.HandleAsync(
            new RemovePlanCommand(
                monthlyBillingId,
                planId
            ),
            token
        );

        return NoContent();
    }
}