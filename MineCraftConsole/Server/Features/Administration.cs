using Microsoft.EntityFrameworkCore;
using MineCraftConsole.Server.Persistence;
using MineCraftConsole.Server.Services;

namespace MineCraftConsole.Server.Features;

public static class Administration
{
  public static WebApplication MapAdministration(this WebApplication app)
  {
    app.MapPost("/administration/versions/refresh", RefreshVersions);
    app.MapGet("/administration/versions", GetVersions);
    return app;
  }

  private async static Task RefreshVersions(LauncherMetaService versionService, ServerDbContext db)
  {
    var versions = await versionService.GetServerVersions();
    foreach (var version in versions)
    {
      db.Attach(version);
    }
    await db.SaveChangesAsync();
  }

  private async static Task<IResult> GetVersions(ServerDbContext db)
  {
    return Results.Ok(await db.ServerVersions.ToListAsync());
  }
}
