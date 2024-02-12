namespace Domain.Exceptions;

public class BusinessException : BaseException
{
    public int Code { get; init; }

    public BusinessException(string message, string source) : base(message, source)
    {
    }
}
