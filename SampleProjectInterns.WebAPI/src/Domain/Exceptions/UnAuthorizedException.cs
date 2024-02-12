namespace Domain.Exceptions;
public class UnAuthorizedException : BaseException
{
    public UnAuthorizedException(string message, string source) : base(message, source)
    {
    }
}
