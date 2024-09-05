using AngryBirds.Email.Core.Models;

namespace AngryBirds.Email.Core;

public interface IEmailProvider
{
    Task<EmailResult> SendEmailAsync(EmailMessage message);
    Task<EmailResult> SendTemplateEmailAsync(string templateId, object templateData, EmailRecipient recipient);
    Task<EmailResult> AddContactAsync(EmailRecipient contact, List<string> listNames);
    Task<EmailResult> RemoveContactAsync(string email);
    Task<EmailResult> CreateListAsync(string listName);
    Task<EmailResult> DeleteListAsync(string listName);
}