using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using MineCraftConsole.Server;
using MineCraftConsole.Server.Api;
using MineCraftConsole.Server.Hubs;
using MineCraftConsole.Server.Services;
using MineCraftConsole.Shared;
using System.Threading.Channels;



var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);


var app = builder.Build();
ConfigureRequestPipeline(app, app.Environment);


app.Run();


static void ConfigureServices(IServiceCollection services)
{
  var channel = Channel.CreateBounded<string>(10);
  services.AddSingleton(channel.Reader);
  services.AddSingleton(channel.Writer);
 
  services.AddSingleton<IServerRunner>(sp => 
  {
    var launchSettings = new ServerLaunchSettings { JarPath = @"c:\mc\server-1.17.jar", Xmx = 1024, Xms = 1024 };
    var pipeSource= new ChannelReaderPipeSource(sp.GetRequiredService<ChannelReader<string>>());
    var pipeTarget = new HubPipeTarget(sp.GetRequiredService<IHubContext<ConsoleHub, IConsoleClient>>());
    return new ServerRunner(launchSettings, pipeSource, pipeTarget);
  });

  services.AddResponseCompression(opts =>
  {
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
  });

  services.AddSignalR();
  services.AddRazorPages();
}

static void ConfigureRequestPipeline(WebApplication app, IWebHostEnvironment env)
{

  // Configure the HTTP request pipeline.
  if (env.IsDevelopment())
  {
    app.UseWebAssemblyDebugging();
  }
  else
  {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

  app.UseHttpsRedirection();

  app.UseBlazorFrameworkFiles();
  app.UseStaticFiles();

  app.UseRouting();
  app.RegisterApi();

  app.UseEndpoints(endpoints =>
  {
    endpoints.MapRazorPages();
    endpoints.MapHub<ConsoleHub>("/consolehub");
    endpoints.MapFallbackToFile("index.html");
  });
}

