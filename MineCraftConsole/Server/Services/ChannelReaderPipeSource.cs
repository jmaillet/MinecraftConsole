using CliWrap;
using System.Threading.Channels;

namespace MineCraftConsole.Server.Services;
public class ChannelReaderPipeSource : PipeSource
{
  private readonly ChannelReader<string> _channelReader;

  public ChannelReaderPipeSource(ChannelReader<string> channelReader)
  {
    _channelReader = channelReader;
  }

  public async override Task CopyToAsync(Stream destination, CancellationToken cancellationToken = default)
  {
    using var sw = new StreamWriter(destination) { AutoFlush = true };
    await foreach(var line in _channelReader.ReadAllAsync(cancellationToken))
    {
      await sw.WriteLineAsync(line);
    }
  }
}
