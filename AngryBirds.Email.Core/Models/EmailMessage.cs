namespace AngryBirds.Email.Core.Models;

public class EmailMessage
{
    public EmailRecipient Recipient { get; set; }
    public EmailSender Sender { get; set; }
    public string Subject { get; set; }
    public string HtmlBody { get; set; }
    public string PlainTextBody { get; set; }
    public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
}

// EmailRecipient.cs
public class EmailRecipient
{
    public string Email { get; set; }
    public string Name { get; set; }
}

// EmailSender.cs
public class EmailSender
{
    public string Email { get; set; }
    public string Name { get; set; }
}

// EmailAttachment.cs
public class EmailAttachment
{
    public string FileName { get; set; }
    public byte[] Content { get; set; }
    public string ContentType { get; set; }
}

// EmailResult.cs
public class EmailResult
{
    public bool IsSuccess { get; set; }
    public string MessageId { get; set; }
    public string ErrorMessage { get; set; }
}