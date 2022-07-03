using Microsoft.AspNetCore.ResponseCompression;
using MineCraftConsole.Server.Persistence;
using MineCraftConsole.Server.Services;

namespace MineCraftConsole.Server.Startup;
public static class ServiceRegistration
{
  public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
  {
    builder.Services.AddSingleton<IServerRunnerFactory, ServerRunnerFactory>();
    builder.Services.AddHttpClient();

    builder.Services.AddResponseCompression(opts => {
      opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
    });

    builder.Services.AddDbContext<ServerDbContext>();

    builder.Services.AddSignalR();
    builder.Services.AddRazorPages();
    return builder;
  }
}
