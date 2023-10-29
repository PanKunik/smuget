namespace Domain.Exceptions;

public class ValidationException : SmugetException
{
    public string FieldName { get; init; }

    public ValidationException(
        string message,
        string fieldName
    )
        : base(message)
    {
        FieldName = fieldName;
    }
}