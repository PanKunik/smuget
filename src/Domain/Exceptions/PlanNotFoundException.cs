using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class PlanNotFoundException : SmugetException
{
    public PlanId PlanId { get; private set; }

    public PlanNotFoundException()
        : base($"Plan with passed id doesn't exist in monthly billing.")
    {
    }
    public PlanNotFoundException(PlanId planId)
        : base($"Plan with id = `{planId}` doesn't exists.")
    {
        PlanId = planId;
    }
}