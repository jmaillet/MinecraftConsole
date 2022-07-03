using Microsoft.EntityFrameworkCore;
using MineCraftConsole.Shared;

namespace MineCraftConsole.Server.Persistence;

public class ServerDbContext : DbContext
{
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    optionsBuilder.UseSqlite($"Data Source={Path.Combine(path, "mcc.db")}");
  }

  public DbSet<ServerInstance> ServerInstances => Set<ServerInstance>();

  public DbSet<ServerVersion> ServerVersions => Set<ServerVersion>();
}
