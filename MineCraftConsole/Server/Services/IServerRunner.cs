
using CliWrap;
using MineCraftConsole.Shared;

namespace MineCraftConsole.Server.Services;

public interface IServerRunner
{
  CommandTask<CommandResult> Start(ServerInstance server);
}