namespace Domain.Exceptions;

public sealed class InvalidPlanIdException : SmugetException
{
    public InvalidPlanIdException()
        : base("Plan id cannot be empty.") { }
}