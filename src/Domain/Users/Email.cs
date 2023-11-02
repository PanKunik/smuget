using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.Users;

public sealed record Email
{
    private static readonly Regex Regex = new(
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        RegexOptions.Compiled);

    private const int MaximumLengthOfEmail = 60;

    public string Value { get; }

    public Email(string value)
    {
        ThrowIfEmailIsNullOrEmpty(value);
        ThrowIfEmailIsTooLong(value);
        ThrowIfEmailIsInvalid(value);
        Value = value;
    }

    private void ThrowIfEmailIsNullOrEmpty(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmailIsEmptyException();
        }
    }

    private void ThrowIfEmailIsTooLong(string value)
    {
        if (value.Length > MaximumLengthOfEmail)
        {
            throw new EmailIsTooLongException(MaximumLengthOfEmail);
        }
    }

    private void ThrowIfEmailIsInvalid(string value)
    {
        if (!Regex.IsMatch(value))
        {
            throw new EmailIsInvalidException();
        }
    }
}