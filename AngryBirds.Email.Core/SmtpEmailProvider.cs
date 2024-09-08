using AngryBirds.Email.Core.Models;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace AngryBirds.Email.Core;

public class SmtpEmailProvider : IEmailProvider
{
    private readonly SmtpEmailConfig _config;

    public SmtpEmailProvider(SmtpEmailConfig config)
    {
        _config = config;
    }

    public async Task<EmailResult> SendEmailAsync(EmailMessage message)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(message.Sender.Name, message.Sender.Email));
        mimeMessage.To.Add(new MailboxAddress(message.Recipient.Name, message.Recipient.Email));
        mimeMessage.Subject = message.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = message.HtmlBody,
            TextBody = message.PlainTextBody
        };

        foreach (var attachment in message.Attachments)
        {
            builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.ContentType));
        }

        mimeMessage.Body = builder.ToMessageBody();

        try
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_config.SmtpServer, _config.Port, _config.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_config.Username, _config.Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }

            return new EmailResult { IsSuccess = true, MessageId = mimeMessage.MessageId };
        }
        catch (Exception ex)
        {
            return new EmailResult { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    // Implement other IEmailProvider methods as needed
    public Task<EmailResult> SendTemplateEmailAsync(string templateId, object templateData, EmailRecipient recipient)
    {
        throw new NotImplementedException("Template emails not supported in SMTP provider");
    }

    public Task<EmailResult> AddContactAsync(EmailRecipient contact, List<string> listNames)
    {
        throw new NotImplementedException("Contact management not supported in SMTP provider");
    }

    public Task<EmailResult> RemoveContactAsync(string email)
    {
        throw new NotImplementedException("Contact management not supported in SMTP provider");
    }

    public Task<EmailResult> CreateListAsync(string listName)
    {
        throw new NotImplementedException("List management not supported in SMTP provider");
    }

    public Task<EmailResult> DeleteListAsync(string listName)
    {
        throw new NotImplementedException("List management not supported in SMTP provider");
    }
}