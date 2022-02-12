using CliWrap;
using CliWrap.EventStream;
using Microsoft.AspNetCore.SignalR;
using MineCraftConsole.Server.Hubs;
using Nerdbank.Streams;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace MineCraftConsole.Server.Services;





public class Coordinator : ICoordinator,IReceiver
{

    private readonly IHubContext<ConsoleHub> _hubContext;
    private readonly ConcurrentQueue<string> _queue = new();

    public Coordinator( IHubContext<ConsoleHub> hubContext)
    {
        _hubContext = hubContext;
    }

    private Task SendAsync(string text)
    {
        return _hubContext.Clients.All.SendAsync("Receive", text);
    }

    public async Task StartServerAsync(CancellationToken stoppingToken)
    {
        try
        {

            var cmd = Cli.Wrap("java")
                .WithArguments(args => args
                              .Add("-Xmx1024M")
                              .Add("-Xms1024M")
                .Add("-jar").Add("c:\\mc\\server-1.17.jar").Add("nogui"))
                .WithWorkingDirectory("c:\\mc")
                .WithStandardInputPipe(new QueuePipeSource(_queue))
                .WithStandardOutputPipe(PipeTarget.ToDelegate(SendAsync));


            var result = await cmd.ExecuteAsync(stoppingToken);


        }
        catch (OperationCanceledException x)
        {
            Console.WriteLine("Cancelled!");
            _queue.Enqueue("stop");
        }
    }

    public Task Receive(string text)
    {
        _queue.Enqueue(text);
        return Task.CompletedTask;
    }
}
