namespace Domain.Exceptions;

public class ValidationException : BaseException
{
    public ValidationException(string message, string source) : base(message, source)
    {
    }
}