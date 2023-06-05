namespace Domain.Exceptions;

public sealed class PlanIdIsNullException : SmugetException
{
    public PlanIdIsNullException()
        : base("PlanId cannot be empty!") { }
}