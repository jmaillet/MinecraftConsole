using MineCraftConsole.Server.Services;
using System.Threading.Channels;

namespace MineCraftConsole.Server.Api;

public static class ServerApi
{
  public static WebApplication RegisterApi(this WebApplication app)
  {
    _ = app.MapPost("api/server/start", Start);
    _ = app.MapPost("api/server/stop", Stop);
    return app;
  }

  private static Task<IResult> Start(IServerRunner serverRunner)
  {
    var cmd = serverRunner.Start();
    return Task.FromResult(Results.Ok(cmd.ProcessId));
  }

  private async static Task<IResult> Stop(ChannelWriter<string> writer)
  {
    await writer.WriteAsync("stop");
    return Results.NoContent();
  }
}
