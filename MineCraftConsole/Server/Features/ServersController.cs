using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MineCraftConsole.Server.Persistence;
using MineCraftConsole.Server.Services;
using System.Threading.Channels;

namespace MineCraftConsole.Server.Features;

[ApiController]
[Route("api/[controller]")]
public class ServersController : ControllerBase
{
  private readonly IServerRunner _serverRunner;

  private readonly ChannelWriter<string> _writer;
  private readonly ServerDbContext _db;

  public ServersController(IServerRunner serverRunner, ChannelWriter<string> writer, ServerDbContext db)
  {
    _serverRunner = serverRunner;
    _writer = writer;
    _db = db;
  }

  [HttpPost("{id}/start")]
  public async Task<ActionResult> Start(int id)
  {
    var server = await _db.ServerInstances.FindAsync(id);
    if (server is null)
    {
      return NotFound();
    }
    var cmdTask = _serverRunner.Start(server);
    server.ProcessId = cmdTask.ProcessId;
    await _db.SaveChangesAsync();
    return Accepted(server.ProcessId);
  }

  [HttpPost("{id}/stop")]
  public async Task<ActionResult> Stop(int id)
  {
    var server = await _db.ServerInstances.FindAsync(id);
    if (server is null)
    {
      return NotFound();
    }
    server.ProcessId = null;
    await _db.SaveChangesAsync();
    await _writer.WriteAsync("stop");
    return NoContent();
  }

  [HttpGet]
  public async Task<ActionResult> Get()
  {
    return Ok(await _db.ServerInstances.ToListAsync());
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> Get(int id)
  {
    var server = await _db.ServerInstances.FindAsync(id);
    return server is null ? NotFound() : Ok(server);
  }

}
