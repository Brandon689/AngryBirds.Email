using AngryBirds.Email.Core;
using AngryBirds.Email.Core.Models;
using AngryBirds.Email.Providers.ElasticEmail;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("AngryBirds Email Console Test");
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
        string apiKey = config["ElasticEmail:ApiKey"];
        string testEmail = config["TestEmail"];
        string templateId = config["templateId"];

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(testEmail))
        {
            Console.WriteLine("API key or test email not found in secrets.");
            return;
        }

        var emailConfig = new ElasticEmailConfig { ApiKey = apiKey };
        var httpClient = new HttpClient();
        var emailProvider = new ElasticEmailProvider(emailConfig, httpClient);
        var emailService = new EmailService(emailProvider);

        await TestSendEmail(emailService, testEmail);
        await TestSendTemplateEmail(emailService, testEmail, templateId */);
        await TestAddContact(emailProvider, testEmail);
        await TestCreateAndDeleteList(emailProvider);

        Console.WriteLine("Tests completed. Press any key to exit.");
        Console.ReadKey();
    }

    static async Task TestSendEmail(EmailService emailService, string testEmail)
    {
        Console.WriteLine("\nTesting Send Email...");
        var message = new EmailMessage
        {
            Recipient = new EmailRecipient { Email = testEmail, Name = "Test User" },
            Sender = new EmailSender { Email = testEmail, Name = "AngryBirds" },
            Subject = "Test Email from AngryBirds Email Library",
            HtmlBody = "<h1>This is a test email</h1><p>Hello from AngryBirds Email Library!</p>",
            PlainTextBody = "This is a test email. Hello from AngryBirds Email Library!"
        };

        var result = await emailService.SendEmailAsync(message);
        Console.WriteLine($"Send Email Result: Success={result.IsSuccess}, MessageId={result.MessageId}, Error={result.ErrorMessage}");
    }

    static async Task TestSendTemplateEmail(EmailService emailService, string testEmail, string templateId)
    {
        Console.WriteLine("\nTesting Send Template Email...");
        var templateData = new { name = "Test User", product = "AngryBirds Email Library" };
        var recipient = new EmailRecipient { Email = testEmail, Name = "Test User" };

        var result = await emailService.SendTemplateEmailAsync(templateId, templateData, recipient);
        Console.WriteLine($"Send Template Email Result: Success={result.IsSuccess}, MessageId={result.MessageId}, Error={result.ErrorMessage}");
    }

    static async Task TestAddContact(IEmailProvider emailProvider, string testEmail)
    {
        Console.WriteLine("\nTesting Add Contact...");
        var contact = new EmailRecipient { Email = testEmail, Name = "Test User" };
        var lists = new List<string> { "TestList" };

        var result = await emailProvider.AddContactAsync(contact, lists);
        Console.WriteLine($"Add Contact Result: Success={result.IsSuccess}, MessageId={result.MessageId}, Error={result.ErrorMessage}");
    }

    static async Task TestCreateAndDeleteList(IEmailProvider emailProvider)
    {
        Console.WriteLine("\nTesting Create and Delete List...");
        var listName = "TestList_" + DateTime.Now.Ticks;

        var createResult = await emailProvider.CreateListAsync(listName);
        Console.WriteLine($"Create List Result: Success={createResult.IsSuccess}, MessageId={createResult.MessageId}, Error={createResult.ErrorMessage}");

        if (createResult.IsSuccess)
        {
            var deleteResult = await emailProvider.DeleteListAsync(listName);
            Console.WriteLine($"Delete List Result: Success={deleteResult.IsSuccess}, MessageId={deleteResult.MessageId}, Error={deleteResult.ErrorMessage}");
        }
    }
}