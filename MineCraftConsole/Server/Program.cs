using Microsoft.AspNetCore.ResponseCompression;
using MineCraftConsole.Server.Hubs;
using MineCraftConsole.Server.Services;
using System.Threading.Channels;

#pragma warning disable IDE0058

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);


var app = builder.Build();
ConfigureRequestPipeline(app, app.Environment);


app.Run();


static void ConfigureServices(IServiceCollection services)
{
  services.AddSingleton(Channel.CreateBounded<string>(100));

  services.AddRazorPages();
  services.AddSignalR();
  services.AddResponseCompression(opts =>
  {
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
  });

}

static void ConfigureRequestPipeline(IApplicationBuilder app, IWebHostEnvironment env)
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

  app.UseEndpoints(endpoints =>
  {
    endpoints.MapRazorPages();
    endpoints.MapHub<ConsoleHub>("/consolehub");
    endpoints.MapFallbackToFile("index.html");
  });
}
#pragma warning restore IDE0058





