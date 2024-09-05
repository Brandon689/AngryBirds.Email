using AngryBirds.Email.Core.Models;

namespace AngryBirds.Email.Core;

public class EmailService
{
    private readonly IEmailProvider _provider;

    public EmailService(IEmailProvider provider)
    {
        _provider = provider;
    }

    public async Task<EmailResult> SendEmailAsync(EmailMessage message)
    {
        return await _provider.SendEmailAsync(message);
    }

    public async Task<EmailResult> SendTemplateEmailAsync(string templateId, object templateData, EmailRecipient recipient)
    {
        return await _provider.SendTemplateEmailAsync(templateId, templateData, recipient);
    }
}