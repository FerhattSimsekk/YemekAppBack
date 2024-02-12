namespace Application.Interfaces.Mailing;

public interface IMailSender
{
    Task SendMail(Mail mail);

    Task SendTemplatedMail(Mail mail, IMailTemplate mailTemplate);
}