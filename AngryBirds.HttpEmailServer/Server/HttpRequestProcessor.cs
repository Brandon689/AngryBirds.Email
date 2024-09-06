using AngryBirds.HttpServer.Configuration;
using AngryBirds.HttpServer.Email;
using System.Net;
using System.Text.Json;

namespace AngryBirds.HttpServer.Server;

internal class HttpRequestProcessor
{
    private readonly EmailSender _emailSender;

    public HttpRequestProcessor(EmailServerConfig config)
    {
        _emailSender = new EmailSender(config);
    }

    public async Task ProcessRequest(HttpListenerContext context)
    {
        if (context.Request.HttpMethod == "POST" && context.Request.Url?.AbsolutePath == "/send-email")
        {
            await HandleSendEmailRequest(context);
        }
        else
        {
            context.Response.StatusCode = 404;
        }

        context.Response.Close();
    }

    private async Task HandleSendEmailRequest(HttpListenerContext context)
    {
        using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
        string requestBody = await reader.ReadToEndAsync();
        var emailMessage = JsonSerializer.Deserialize<EmailMessage>(requestBody);

        if (emailMessage != null)
        {
            await _emailSender.SendEmailAsync(emailMessage);
            context.Response.StatusCode = 200;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Email sent successfully");
            await context.Response.OutputStream.WriteAsync(buffer);
        }
        else
        {
            context.Response.StatusCode = 400;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Invalid email data");
            await context.Response.OutputStream.WriteAsync(buffer);
        }
    }
}