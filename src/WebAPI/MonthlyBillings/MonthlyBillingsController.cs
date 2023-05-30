using Application.Abstractions.CQRS;
using Application.MonthlyBillings.Commands.AddExpense;
using Application.MonthlyBillings.Commands.AddIncome;
using Application.MonthlyBillings.Commands.AddPlan;
using Application.MonthlyBillings.Commands.OpenMonthlyBilling;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

[ApiController]
[Route("api/monthlyBillings")]
public sealed class MonthlyBillingsController : ControllerBase
{
    private readonly ICommandHandler<OpenMonthlyBillingCommand> _openMonthlyBilling;
    private readonly ICommandHandler<AddIncomeCommand> _addIncome;
    private readonly ICommandHandler<AddPlanCommand> _addPlan;
    private readonly ICommandHandler<AddExpenseCommand> _addExpense;

    public MonthlyBillingsController(
        ICommandHandler<OpenMonthlyBillingCommand> openMonthlyBilling,
        ICommandHandler<AddIncomeCommand> addIncome,
        ICommandHandler<AddPlanCommand> addPlan,
        ICommandHandler<AddExpenseCommand> addExpense
    )
    {
        _openMonthlyBilling = openMonthlyBilling;
        _addIncome = addIncome;
        _addPlan = addPlan;
        _addExpense = addExpense;
    }

    [HttpPost]
    public async Task<IActionResult> Open(
        OpenMonthlyBillingRequest request,
        CancellationToken token = default)
    {
        var (year, month) = request;

        await _openMonthlyBilling.HandleAsync(
            new OpenMonthlyBillingCommand(year, month),
            token);

        return Created("", null);
    }

    [HttpPost("{id}/incomes")]
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

    [HttpPost("{id}/plans")]
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

    [HttpPost("{id}/plans/{planId}/expenses")]
    public async Task<IActionResult> AddExpense(
        [FromRoute(Name = "id")] Guid monthlyBillingId,
        [FromRoute(Name = "planId")] Guid planId,
        [FromBody] AddExpenseRequest request,
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
}