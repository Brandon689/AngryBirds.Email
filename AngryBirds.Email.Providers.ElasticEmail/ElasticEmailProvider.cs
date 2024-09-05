using AngryBirds.Email.Core;
using AngryBirds.Email.Core.Models;
using System.Text.Json;

namespace AngryBirds.Email.Providers.ElasticEmail;

public class ElasticEmailProvider : IEmailProvider
{
    private readonly HttpClient _httpClient;
    private readonly ElasticEmailConfig _config;

    public ElasticEmailProvider(ElasticEmailConfig config, HttpClient httpClient)
    {
        _config = config;
        _httpClient = httpClient;
    }

    public async Task<EmailResult> SendEmailAsync(EmailMessage message)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("apikey", _config.ApiKey),
            new KeyValuePair<string, string>("to", message.Recipient.Email),
            new KeyValuePair<string, string>("from", message.Sender.Email),
            new KeyValuePair<string, string>("subject", message.Subject),
            new KeyValuePair<string, string>("body_html", message.HtmlBody),
            new KeyValuePair<string, string>("body_text", message.PlainTextBody)
        });

        return await SendRequestAsync("email/send", content);
    }

    public async Task<EmailResult> SendTemplateEmailAsync(string templateId, object templateData, EmailRecipient recipient)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("apikey", _config.ApiKey),
            new KeyValuePair<string, string>("to", recipient.Email),
            new KeyValuePair<string, string>("template", templateId),
            new KeyValuePair<string, string>("merge_source", "json"),
            new KeyValuePair<string, string>("merge_data", JsonSerializer.Serialize(templateData))
        });

        return await SendRequestAsync("email/send", content);
    }

    public async Task<EmailResult> AddContactAsync(EmailRecipient contact, List<string> listNames)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("apikey", _config.ApiKey),
            new KeyValuePair<string, string>("email", contact.Email),
            new KeyValuePair<string, string>("name", contact.Name),
            new KeyValuePair<string, string>("listnames", string.Join(";", listNames))
        });

        return await SendRequestAsync("contact/add", content);
    }

    public async Task<EmailResult> RemoveContactAsync(string email)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("apikey", _config.ApiKey),
            new KeyValuePair<string, string>("email", email)
        });

        return await SendRequestAsync("contact/delete", content);
    }

    public async Task<EmailResult> CreateListAsync(string listName)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("apikey", _config.ApiKey),
            new KeyValuePair<string, string>("listname", listName)
        });

        return await SendRequestAsync("list/add", content);
    }

    public async Task<EmailResult> DeleteListAsync(string listName)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("apikey", _config.ApiKey),
            new KeyValuePair<string, string>("listname", listName)
        });

        return await SendRequestAsync("list/delete", content);
    }

    private async Task<EmailResult> SendRequestAsync(string endpoint, FormUrlEncodedContent content)
    {
        var response = await _httpClient.PostAsync($"{_config.ApiEndpoint}{endpoint}", content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new EmailResult { IsSuccess = true, MessageId = responseString };
        }
        else
        {
            return new EmailResult { IsSuccess = false, ErrorMessage = responseString };
        }
    }
}