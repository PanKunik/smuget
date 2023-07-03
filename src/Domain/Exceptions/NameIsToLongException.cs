namespace Domain.Exceptions;

public sealed class NameIsToLongException : SmugetException
{
    public NameIsToLongException(int length, byte maxLength)
        : base($"Name is too long. It can contains max. {maxLength} characters. You passed: {length} characters.") { }
}