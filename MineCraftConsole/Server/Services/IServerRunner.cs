
using CliWrap;

namespace MineCraftConsole.Server.Services;

public interface IServerRunner
{
  CommandTask<CommandResult> Start();
}