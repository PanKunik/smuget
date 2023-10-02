namespace Domain.Exceptions;

public sealed class IncomeNotFoundException : SmugetException
{
    public IncomeNotFoundException()
        : base("Income with passed id doesn't exist in monthly billing.") { }
}