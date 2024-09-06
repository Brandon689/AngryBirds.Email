using AngryBirds.HttpServer.Configuration;
using System.Net;

namespace AngryBirds.HttpServer.Server;

public class HttpEmailServer
{
    private readonly HttpListener _listener;
    private readonly HttpRequestProcessor _requestProcessor;

    public HttpEmailServer(string prefix, EmailServerConfig config)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add(prefix);
        _requestProcessor = new HttpRequestProcessor(config);
    }

    public async Task StartAsync()
    {
        _listener.Start();
        Console.WriteLine("Server started. Listening for requests...");

        while (true)
        {
            HttpListenerContext context = await _listener.GetContextAsync();
            _ = Task.Run(() => _requestProcessor.ProcessRequest(context));
        }
    }

    public void Stop()
    {
        _listener.Stop();
    }
}