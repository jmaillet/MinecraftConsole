using CliWrap;
using Microsoft.AspNetCore.SignalR;
using MineCraftConsole.Server.Hubs;
using MineCraftConsole.Shared;

namespace MineCraftConsole.Server.Services;

public class HubPipeTarget : PipeTarget
{
  private readonly IHubContext<ConsoleHub, IConsoleClient> _context;

  public HubPipeTarget(IHubContext<ConsoleHub, IConsoleClient> context)
  {
    _context = context;
  }

  public async override Task CopyFromAsync(Stream origin, CancellationToken cancellationToken = default)
  {
    var sr = new StreamReader(origin);
    string? line;
    while ((line = await sr.ReadLineAsync()) != null)
    {
      await _context.Clients.All.ReceiveLine(line);
    }
  }
}
