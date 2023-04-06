using Application.Abstractions.CQRS;
using Application.MonthlyBillings.Commands.AddIncome;
using Application.MonthlyBillings.Commands.OpenMonthlyBilling;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

[ApiController]
[Route("api/monthlyBillings")]
public sealed class MonthlyBillingsController : ControllerBase
{
    private readonly ICommandHandler<OpenMonthlyBillingCommand> _openMonthlyBilling;
    private readonly ICommandHandler<AddIncomeCommand> _addIncome;

    public MonthlyBillingsController(
        ICommandHandler<OpenMonthlyBillingCommand> openMonthlyBilling,
        ICommandHandler<AddIncomeCommand> addIncome
    )
    {
        _openMonthlyBilling = openMonthlyBilling;
        _addIncome = addIncome;
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
}