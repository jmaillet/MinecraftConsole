using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MineCraftConsole.Shared;
public class ServerInstance
{
  [JsonConstructor]
  public ServerInstance(int id, string name, string serverJarPath, string xmx, string xms)
  {
    Id = id;
    Name = name;
    ServerJarPath = serverJarPath;
    Xmx = xmx;
    Xms = xms;
  }

  public ServerInstance(string name, string serverJarPath, string xmx, string xms) 
    : this(0, name, serverJarPath, xmx, xms) { }

  [Key]
  [Required, Range(1, int.MaxValue)]
  public int Id { get; private set; }

  [Required]
  public string Name { get; set; }

  [Required]
  public string ServerJarPath { get; set; }
  public string Xmx { get; set; }
  public string Xms { get; set; }

  public int? ProcessId { get; set; }
}
