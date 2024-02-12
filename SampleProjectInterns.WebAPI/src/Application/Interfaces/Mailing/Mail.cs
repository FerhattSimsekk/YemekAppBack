namespace Application.Interfaces.Mailing;

public class Mail
{
    public IReadOnlyList<MailAddress> From { get; set; } = new List<MailAddress>();
    public IReadOnlyList<MailAddress> To { get; set; } = new List<MailAddress>();

    public string Subject { get; set; } = null!;
    public MailBody Body { get; set; } = null!;
}