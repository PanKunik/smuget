namespace Domain.Exceptions;

public sealed class PlanIsNullException : SmugetException
{
    public PlanIsNullException()
        : base("Plan cannot be empty!") { }
}