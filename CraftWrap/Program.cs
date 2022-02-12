
using CliWrap;
using CliWrap.EventStream;
using System.Collections.Concurrent;

using static System.Console;

namespace CraftWrap;

public class Program
{
  public static async Task Main(string[] args)
  {



        var cmd = Cli.Wrap("java")
        .WithArguments(args => args
                      .Add("-Xmx1024M")
                      .Add("-Xms1024M")
        .Add("-jar").Add("c:\\mc\\server-1.17.jar").Add("nogui"))
        .WithWorkingDirectory("c:\\mc")
        .WithStandardInputPipe(PipeSource.FromStream(OpenStandardInput()))
        .WithStandardOutputPipe(PipeTarget.ToStream(OpenStandardOutput()));

        var process =  cmd.ExecuteAsync();

  
  }
}







