using AngryBirds.HttpServer.Configuration;
using AngryBirds.HttpServer.Server;
using System.Text.Json;
using System.Text;
using Xunit;

namespace AngryBirds.HttpServer;

public class HttpEmailServerTests
{
    [Fact]
    public async Task TestServerSendEmail()
    {
        var config = new EmailServerConfig
        {
            SmtpServer = "smtp.zoho.com",
            SmtpPort = 587,
            SmtpUsername = "your_zoho_email@zoho.com",
            SmtpPassword = "your_zoho_password",
            SenderName = "Test Sender",
            SenderEmail = "your_zoho_email@zoho.com"
        };

        var server = new HttpEmailServer("http://localhost:8080/", config);

        // Start the server in a separate task
        var serverTask = Task.Run(() => server.StartAsync());

        // Wait a bit for the server to start
        await Task.Delay(1000);

        try
        {
            // Send a test email request to the server
            using (var client = new HttpClient())
            {
                var emailRequest = new
                {
                    To = "recipient@example.com",
                    Subject = "Test Email from HttpEmailServer",
                    Body = "<h1>Test Email</h1><p>This is a test email sent from HttpEmailServer.</p>"
                };

                var content = new StringContent(JsonSerializer.Serialize(emailRequest), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:8080/send-email", content);

                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

                var responseBody = await response.Content.ReadAsStringAsync();
                Assert.Contains("Email sent successfully", responseBody);
            }
        }
        finally
        {
            // Stop the server
            server.Stop();

            // Wait for the server task to complete
            await serverTask;
        }
    }
}

//public class HttpEmailServerTests
//{
//    [Fact]
//    public async Task TestServerStart()
//    {
//        var config = new EmailServerConfig
//        {
//            SmtpServer = "smtp.example.com",
//            SmtpPort = 587,
//            SmtpUsername = "username",
//            SmtpPassword = "password",
//            SenderName = "Test Sender",
//            SenderEmail = "sender@example.com"
//        };

//        var server = new HttpEmailServer("http://localhost:8080/", config);

//        // Start the server in a separate task
//        var serverTask = Task.Run(() => server.StartAsync());

//        // Wait a bit for the server to start
//        await Task.Delay(1000);

//        // Stop the server
//        server.Stop();

//        // Wait for the server task to complete
//        await serverTask;

//        // If we reach here without exceptions, the test passes
//        Assert.True(true);
//    }
//}