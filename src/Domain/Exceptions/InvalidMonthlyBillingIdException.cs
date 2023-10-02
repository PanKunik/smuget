namespace Domain.Exceptions;

public sealed class InvalidMonthlyBillingIdException : SmugetException
{
    public InvalidMonthlyBillingIdException()
        : base("Monthly billing id cannot be empty.") { }
}