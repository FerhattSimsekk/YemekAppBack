using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;
using Application.Interfaces.Mailing;
using Infrastructure.Settings;
using System.Net.Sockets;
using System.Text.Json;

namespace Infrastructure;

public class MailSenderMailKit : IMailSender
{
    private readonly IOptions<MailServerSettings> _options;
    private readonly ILogger<MailSenderMailKit> _logger;

    public MailSenderMailKit(IOptions<MailServerSettings> options, ILogger<MailSenderMailKit> logger)
    {
        _options = options;
        _logger = logger;
    }

    public Task SendMail(Mail mail)
    {
        //TODO: Right now, it is fire and forget.Replace with event bus
        Task.Run(async () => await SendMailWithMailKit(mail));

        return Task.CompletedTask;
    }

    public Task SendTemplatedMail(Mail mail, IMailTemplate mailTemplate)
    {
        throw new NotImplementedException();
    }

    private async Task SendMailWithMailKit(Mail mail)
    {
        try
        {
            var message = new MimeMessage();

            var fromAddresses = mail.From.Count > 0
                ? mail.From.Select(address => new MailboxAddress(address.Name, address.Email))
                : new List<MailboxAddress>() { new MailboxAddress(_options.Value.DisplayName, _options.Value.UserName) };

            message.From.AddRange(fromAddresses);
            message.To.AddRange(mail.To.Select(address => new MailboxAddress(address.Name, address.Email)));
            message.Subject = mail.Subject;

            message.Body = new TextPart(mail.Body.Type == MailBodyType.Text ? "plain" : "html")
            {
                Text = mail.Body.Text
            };

            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await Policy
                .Handle<Exception>()
                .Or<InvalidOperationException>()
                .Or<ObjectDisposedException>()
                .Or<OperationCanceledException>()
                .Or<SocketException>()
                .Or<IOException>()
                .Or<ProtocolException>()
                .Or<AuthenticationException>()
                .Or<SaslException>()
                .Or<ServiceNotConnectedException>()
                .Or<ServiceNotAuthenticatedException>()
                .Or<CommandException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
                    onRetry: (_, _, retryCount, _) => _logger.LogWarning("Mail failed on the {RetryCount}th try", retryCount))
                .ExecuteAsync(async () =>
                {
                    await client.ConnectAsync(_options.Value.Host, _options.Value.Port, _options.Value.UseSsl);

                    await client.AuthenticateAsync(_options.Value.UserName, _options.Value.Password);

                    await client.SendAsync(message);

                    _logger.LogInformation("Mail sent to {MailTo}", JsonSerializer.Serialize(mail.To));
                });
            client.Disconnect(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Mail can not be sent to {MailTo}", JsonSerializer.Serialize(mail.To));
        }
    }
}