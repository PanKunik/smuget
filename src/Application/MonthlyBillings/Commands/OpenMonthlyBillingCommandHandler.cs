using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.Commands;

public sealed class OpenMonthlyBillingCommandHandler : ICommandHandler<OpenMonthlyBillingCommand>
{
    public async Task HandleAsync(OpenMonthlyBillingCommand command, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }
}