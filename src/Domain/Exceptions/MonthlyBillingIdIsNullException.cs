namespace Domain.Exceptions;

public sealed class MonthlyBillingIdIsNullException : SmugetException
{
    public MonthlyBillingIdIsNullException()
        : base("Monthly billing id cannot be null or empty.") { }
}