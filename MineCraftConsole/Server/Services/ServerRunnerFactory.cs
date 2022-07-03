using Microsoft.AspNetCore.SignalR;
using MineCraftConsole.Server.Hubs;
using MineCraftConsole.Shared;
using System.Threading.Channels;

namespace MineCraftConsole.Server.Services;

public class ServerRunnerFactory : IServerRunnerFactory
{
  private readonly IHubContext<ConsoleHub, IConsoleClient> _hubContext;

  public ServerRunnerFactory(IHubContext<ConsoleHub, IConsoleClient> hubContext)
  {
    _hubContext = hubContext;
  }

  public IServerRunner CreateRunner()
  {
    var channel = Channel.CreateBounded<string>(10);
    var pipeSource = new ChannelReaderPipeSource(channel.Reader);
    var pipeTarget = new HubPipeTarget(_hubContext);
    return new ServerRunner(pipeSource, pipeTarget);
  }
}
