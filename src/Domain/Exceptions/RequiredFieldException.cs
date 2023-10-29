namespace Domain.Exceptions;

public abstract class RequiredFieldException : ValidationException
{
    public RequiredFieldException(string fieldName)
        : base(
            $"Field `{fieldName}` is required.",
            fieldName
        ) { }
}