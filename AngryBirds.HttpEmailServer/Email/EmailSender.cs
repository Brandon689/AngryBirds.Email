using AngryBirds.HttpServer.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AngryBirds.HttpServer.Email;

internal class EmailSender
{
    private readonly EmailServerConfig _config;

    public EmailSender(EmailServerConfig config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_config.SenderName, _config.SenderEmail));
        message.To.Add(new MailboxAddress("", emailMessage.To));
        message.Subject = emailMessage.Subject;

        message.Body = new TextPart("html")
        {
            Text = emailMessage.Body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config.SmtpServer, _config.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_config.SmtpUsername, _config.SmtpPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}