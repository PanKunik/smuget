namespace Domain.Exceptions;

public abstract class SmugetException : Exception
{
    protected SmugetException(string message)
     : base(message) { }
}