using Microsoft.EntityFrameworkCore;
using MineCraftConsole.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineCraftConsole.Server.Persistence;
public class ServerDbContext : DbContext
{
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    optionsBuilder.UseSqlite($"Data Source={Path.Combine(path, "mcc.db")}");
  }

  public DbSet<ServerInstance> ServerInstances => Set<ServerInstance>();

}
