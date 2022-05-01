using CliWrap;

namespace MineCraftConsole.Server.Services;
public class ServerRunner : IServerRunner
{
  private readonly ServerLaunchSettings _launchSettings;
  private readonly PipeSource _pipeSource;
  private readonly PipeTarget _pipeTarget;

  public ServerRunner(ServerLaunchSettings launchSettings, PipeSource pipeSource, PipeTarget pipeTarget)
  {
    _launchSettings = launchSettings;
    _pipeSource = pipeSource;
    _pipeTarget = pipeTarget;
  }

  public CommandTask<CommandResult> Start()
  {
    var workingDir = Path.GetDirectoryName(_launchSettings.JarPath);

    return Cli.Wrap("java")
      .WithArguments(args => args
        .Add($"-Xmx{_launchSettings.Xmx}M")
        .Add($"-Xms{_launchSettings.Xms}M")
        .Add("-jar")
        .Add(_launchSettings.JarPath)
        .Add("nogui"))
      .WithWorkingDirectory(workingDir!)
      .WithStandardInputPipe(_pipeSource)
      .WithStandardOutputPipe(_pipeTarget)
      .WithStandardErrorPipe(_pipeTarget)
      .ExecuteAsync();
   
  }
}
