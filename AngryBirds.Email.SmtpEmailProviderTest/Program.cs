using AngryBirds.Email.Core.Models;
using AngryBirds.Email.Core;
using Microsoft.Extensions.Configuration;

var smtpTests = new SmtpEmailProviderTests();
await smtpTests.RunTests();

public class SmtpEmailProviderTests
{
    private readonly IConfiguration _configuration;
    private readonly SmtpEmailConfig _smtpConfig;
    private readonly string _testEmail;

    public SmtpEmailProviderTests()
    {
        _configuration = new ConfigurationBuilder()
            .AddUserSecrets<SmtpEmailProviderTests>()
            .Build();

        _smtpConfig = new SmtpEmailConfig
        {
            SmtpServer = _configuration["Smtp:Server"],
            Port = int.Parse(_configuration["Smtp:Port"]),
            Username = _configuration["Smtp:Username"],
            Password = _configuration["Smtp:Password"],
            UseSsl = bool.Parse(_configuration["Smtp:UseSsl"])
        };

        _testEmail = _configuration["TestEmail"];
    }

    public async Task RunTests()
    {
        Console.WriteLine("Starting SMTP Email Provider Tests");

        var emailProvider = new SmtpEmailProvider(_smtpConfig);
        var emailService = new EmailService(emailProvider);

        await TestSendEmail(emailService);

        Console.WriteLine("SMTP Email Provider Tests Completed");
    }

    private async Task TestSendEmail(EmailService emailService)
    {
        Console.WriteLine("\nTesting Send Email...");
        var message = new EmailMessage
        {
            Recipient = new EmailRecipient { Email = _testEmail, Name = "Test User" },
            Sender = new EmailSender { Email = _smtpConfig.Username, Name = "AngryBirds SMTP Test" },
            Subject = "Test Email from AngryBirds SMTP Email Provider",
            HtmlBody = "<h1>This is a test email</h1><p>Hello from AngryBirds SMTP Email Provider!</p>",
            PlainTextBody = "This is a test email. Hello from AngryBirds SMTP Email Provider!"
        };

        var result = await emailService.SendEmailAsync(message);
        Console.WriteLine($"Send Email Result: Success={result.IsSuccess}, MessageId={result.MessageId}, Error={result.ErrorMessage}");
    }
}