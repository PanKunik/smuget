namespace Domain.Exceptions;

public sealed class IncomeNameNotUniqueException : SmugetException
{
    public IncomeNameNotUniqueException(string name)
        : base($"Income`s name `{ name }` already exists in monthly billing.") { }
}