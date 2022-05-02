using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using MineCraftConsole.Server.Hubs;
using MineCraftConsole.Server.Services;
using MineCraftConsole.Shared;
using System.Threading.Channels;
using MineCraftConsole.Server.Persistence;

namespace MineCraftConsole.Server.Startup;
public static class ServiceRegistration
{
  public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
  {
    var channel = Channel.CreateBounded<string>(10);
    builder.Services.AddSingleton(channel.Reader);
    builder.Services.AddSingleton(channel.Writer);

    // TODO: Make a factory
    builder.Services.AddSingleton<IServerRunner>(sp =>
    {

      var pipeSource = new ChannelReaderPipeSource(sp.GetRequiredService<ChannelReader<string>>());
      var pipeTarget = new HubPipeTarget(sp.GetRequiredService<IHubContext<ConsoleHub, IConsoleClient>>());
      return new ServerRunner(pipeSource, pipeTarget);
    });

    builder.Services.AddResponseCompression(opts =>
    {
      opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
    });

    builder.Services.AddDbContext<ServerDbContext>();

    builder.Services.AddSignalR();
    builder.Services.AddRazorPages();
    return builder;
  }
}
