using AngryBirds.HttpServer.Configuration;
using AngryBirds.HttpServer.Server;
using Xunit;

namespace AngryBirds.HttpServer;

public class HttpEmailServerTests
{
    [Fact]
    public async Task TestServerStart()
    {
        var config = new EmailServerConfig
        {
            SmtpServer = "smtp.example.com",
            SmtpPort = 587,
            SmtpUsername = "username",
            SmtpPassword = "password",
            SenderName = "Test Sender",
            SenderEmail = "sender@example.com"
        };

        var server = new HttpEmailServer("http://localhost:8080/", config);

        // Start the server in a separate task
        var serverTask = Task.Run(() => server.StartAsync());

        // Wait a bit for the server to start
        await Task.Delay(1000);

        // Stop the server
        server.Stop();

        // Wait for the server task to complete
        await serverTask;

        // If we reach here without exceptions, the test passes
        Assert.True(true);
    }
}