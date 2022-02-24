
using CliWrap;

namespace CraftWrap;

public class Program
{
  public static async Task Main(string[] args)
  {

        var listener = new SocketListener();

        var cmd = Cli.Wrap("java")
        .WithArguments(args => args
                      .Add("-Xmx1024M")
                      .Add("-Xms1024M")
        .Add("-jar").Add("c:\\mc\\server-1.17.jar").Add("nogui"))
        .WithWorkingDirectory("c:\\mc")
        .WithStandardInputPipe(new SocketPipeSource(listener.ClientSocket))
        .WithStandardOutputPipe(new SocketPipeTarget(listener.ClientSocket));

        var cts = new CancellationTokenSource();

        var minecraftServerTask = cmd.ExecuteAsync(cts.Token);
        int processId = minecraftServerTask.ProcessId;

        await Task.WhenAll(listener.ListenAsync(cts.Token), minecraftServerTask);
        

  
  }
}







