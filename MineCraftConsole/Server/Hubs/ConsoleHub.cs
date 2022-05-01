using Microsoft.AspNetCore.SignalR;
using MineCraftConsole.Shared;
using System.Threading.Channels;

namespace MineCraftConsole.Server.Hubs;

public class ConsoleHub : Hub<IConsoleClient>
{
  private readonly ChannelWriter<string> _channelWriter;

  public ConsoleHub(ChannelWriter<string> channelWriter)
  {
    _channelWriter = channelWriter;
  }
  public async Task Send(string message) => await _channelWriter.WriteAsync(message);


 
}
