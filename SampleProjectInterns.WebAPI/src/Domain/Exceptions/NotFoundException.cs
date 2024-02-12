namespace Domain.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message, string source) : base(message, source)
    {
    }
}