using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MineCraftConsole.Server.Services;
public class SocketClient : IDisposable
{
    private readonly ILogger<SocketClient> _logger;
    private readonly ChannelReader<string> _reader;
    private readonly ChannelWriter<string> _writer;
    private bool _disposedValue;

    // TODO: get from config
    const string SOCKET_ADDR = $"d:\\temp.socket";

    private Socket? _client;

    public SocketClient(ILogger<SocketClient> logger, Channel<string> channel)
    {
        _logger = logger;
        _reader = channel.Reader;
        _writer = channel.Writer;
    }

    public async Task ConnectAsync(CancellationToken cancellationToken)
    {
        var remoteEP = new UnixDomainSocketEndPoint(SOCKET_ADDR);

        _client = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Unspecified);

        _logger.LogInformation($"Connecting to {SOCKET_ADDR}");
        await _client.ConnectAsync(remoteEP, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Connected");
       
        var stream = new NetworkStream(_client);
        var streamReader = new StreamReader(stream);
        var streamWriter = new StreamWriter(stream) { AutoFlush = true };

        _ = await Task.WhenAny(
            StartReadAsync(streamReader, cancellationToken), 
            StartWriteAsync(streamWriter, cancellationToken)
        );

        _logger.LogDebug("Exit Connect");
       
    }

    private async Task StartReadAsync(StreamReader streamReader, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var line = await streamReader.ReadLineAsync().WaitAsync(cancellationToken).ConfigureAwait(false);
            if (line == null) break;
            await _writer.WriteAsync(line, cancellationToken).ConfigureAwait(false);
        }
        _logger.LogDebug("Exit Read");
    }

    private async Task StartWriteAsync(StreamWriter streamWriter, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var line = await _reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            await streamWriter.WriteLineAsync(line).WaitAsync(cancellationToken).ConfigureAwait(false);

        }
        _logger.LogDebug("Exit Write");
        
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                if (_client?.Connected == true)
                {
                    _client?.Shutdown(SocketShutdown.Both);
                    _client?.Close();
                }
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            _client = null;
            _disposedValue = true;
        }
    }

   

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
