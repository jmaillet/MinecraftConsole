using CliWrap;
using System.Net.Sockets;

namespace CraftWrap;
public class SocketPipeTarget : PipeTarget
{
    private readonly Socket _client;

    public SocketPipeTarget(Socket client)
    {
        _client = client; 
    }

    public override async Task CopyFromAsync(Stream source, CancellationToken cancellationToken = default)
    {
        using var sourceReader = new StreamReader(source);
        StreamWriter? socketWriter = null;

        while(!cancellationToken.IsCancellationRequested)
        {
            if (_client.Connected)
            {
                if(socketWriter == null)
                {
                    socketWriter = new StreamWriter(new NetworkStream(_client)) { AutoFlush = true };
                }
                else
                {
                    await socketWriter.WriteLineAsync(await sourceReader.ReadLineAsync().ConfigureAwait(false)).WaitAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}
