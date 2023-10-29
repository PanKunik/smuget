namespace Domain.Exceptions;

public class InvalidFieldLengthException
    : ValidationException
{
    public InvalidFieldLengthException(
        string fieldName,
        int maxLength
    )
        : base(
            $"Field `{fieldName}` must have length between 1 and {maxLength} characters.",
            fieldName
        ) { }

    public InvalidFieldLengthException(
        string fieldName,
        int minLength,
        int maxLength
    )
        : base(
            $"Field `{fieldName}` must have length between {minLength} and {maxLength} characters.",
            fieldName
        ) { }
}