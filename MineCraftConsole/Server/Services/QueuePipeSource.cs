using CliWrap;
using System.Collections.Concurrent;
using System.Text;

namespace MineCraftConsole.Server.Services
{
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
                var writer = new StreamWriter(destination) { AutoFlush = true };
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (_queue.Count > 0)
                    {
                        if (_queue.TryDequeue(out var text))
                        {
                            Console.WriteLine(text);
                            await writer.WriteLineAsync(text);
                        };

                    }
                }
            });
        }
    }

    public static class QueuePipeSourceExtensions
    {

    }
}
