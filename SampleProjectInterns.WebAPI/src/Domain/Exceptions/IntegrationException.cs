namespace Domain.Exceptions;

public class IntegrationException : Exception
{
    public string Application { get; init; }
    public int Code { get; init; }

    public IntegrationException(string application, int code)
    {
        Application = application;
        Code = code;
    }

    public IntegrationException(string application, int code, string message)
        : base(message)
    {
        Application = application;
        Code = code;
    }
}
