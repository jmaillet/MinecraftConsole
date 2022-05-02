using CliWrap;
using MineCraftConsole.Shared;

namespace MineCraftConsole.Server.Services;
public class ServerRunner : IServerRunner
{

  private readonly PipeSource _pipeSource;
  private readonly PipeTarget _pipeTarget;

  public ServerRunner(PipeSource pipeSource, PipeTarget pipeTarget)
  {
  
    _pipeSource = pipeSource;
    _pipeTarget = pipeTarget;
  }

  public CommandTask<CommandResult> Start(ServerInstance settings)
  {
    var workingDir = Path.GetDirectoryName(settings.ServerJarPath);

    return Cli.Wrap("java")
      .WithArguments(args => args
        .Add($"-Xmx{settings.Xmx}")
        .Add($"-Xms{settings.Xms}")
        .Add("-jar")
        .Add(settings.ServerJarPath)
        .Add("nogui"))
      .WithWorkingDirectory(workingDir!)
      .WithStandardInputPipe(_pipeSource)
      .WithStandardOutputPipe(_pipeTarget)
      .WithStandardErrorPipe(_pipeTarget)
      .ExecuteAsync();
   
  }
}
