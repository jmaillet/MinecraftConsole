
using CliWrap;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace CraftWrap;

public class QueuePipeSource : PipeSource
{
  private readonly ConcurrentQueue<string> _queue;
  public QueuePipeSource(ConcurrentQueue<string> queue)
  {
    _queue = queue;
  }
  public override async Task CopyToAsync(Stream destination, CancellationToken cancellationToken = default)
  {
    await Task.Run(async () =>
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        if (_queue.Count > 0)
        {
          if (_queue.TryDequeue(out var text))
          {
            Debug.WriteLine(destination.GetType());
            
            var bytes = Console.InputEncoding.GetBytes(text);
            await destination.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
            await destination.FlushAsync().ConfigureAwait(false);
          };

        }
      }
    }, cancellationToken);

  }
}

//const string socketPath = @"c:\mc\io.sock";

//if (File.Exists(socketPath))
//{
//  File.Delete(socketPath);
//}

//async Task<Socket> Listen()
//{
//  var endpoint = new UnixDomainSocketEndPoint(socketPath);
//  using var sock = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
//  sock.Bind(endpoint);
//  sock.Listen(1);

//  while (true)
//  {
//    return await sock.AcceptAsync();
//  }

//}






