
using CliWrap;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;

namespace CraftWrap;

public class SocketPipeSource : PipeSource
{
    private readonly Socket _client;

    public SocketPipeSource(Socket client)
    {
        _client = client;
    }

    public override async Task CopyToAsync(Stream destination, CancellationToken cancellationToken = default)
    {
        using var destinationWriter = new StreamWriter(destination) { AutoFlush = true };
        StreamReader? socketReader = null;

        while (!cancellationToken.IsCancellationRequested)
        {
            if (_client.Connected)
            {
                if (socketReader == null)
                {
                    socketReader = new StreamReader(new NetworkStream(_client));
                }
                else
                {
                    await destinationWriter.WriteLineAsync(await socketReader.ReadLineAsync().WaitAsync(cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);
                }
            }
        }
    }
}







