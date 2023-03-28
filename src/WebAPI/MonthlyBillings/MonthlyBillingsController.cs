using Application.Abstractions.CQRS;
using Application.MonthlyBillings.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.MonthlyBillings;

[ApiController]
[Route("api/monthlyBillings")]
public sealed class MonthlyBillingsController : ControllerBase
{
    private readonly ICommandHandler<OpenMonthlyBillingCommand> _openMonthlyBilling;

    public MonthlyBillingsController(ICommandHandler<OpenMonthlyBillingCommand> openMonthlyBilling)
    {
        _openMonthlyBilling = openMonthlyBilling;
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
}