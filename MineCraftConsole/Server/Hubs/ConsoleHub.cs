using Microsoft.AspNetCore.SignalR;
using MineCraftConsole.Server.Services;
using System.Threading.Channels;

namespace MineCraftConsole.Server.Hubs
{
    public class ConsoleHub : Hub
    {
        private readonly Channel<string> _channel;

        public ConsoleHub(Channel<string> channel)
        {
            _channel = channel;
        }

        public async Task WriteMessageStream(ChannelReader<string> stream)
        {
            while (await stream.WaitToReadAsync())
            {
                var line = await stream.ReadAsync();
                await _channel.Writer.WriteAsync(line);

            }
        }

        public ChannelReader<string> ReadMessageStream()
        {
            return _channel.Reader;
        }
    }
}
