namespace Domain.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException(string message, string source) : base(message)
    {
        Source = source;
    }
}