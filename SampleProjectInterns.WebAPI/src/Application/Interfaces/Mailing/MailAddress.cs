namespace Application.Interfaces.Mailing;

public class MailAddress
{
    public MailAddress(string email)
    {
        Email = email;
        Name = email;
    }

    public MailAddress(string email, string name)
    {
        Email = email;
        Name = name;
    }

    public string Email { get; }
    public string Name { get; }
}