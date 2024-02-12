namespace Application.Interfaces.Mailing;

public class MailBody
{
    public MailBody(MailBodyType type)
    {
        Type = type;
    }

    public string Text { get; set; } = null!;
    public MailBodyType Type { get; }
}