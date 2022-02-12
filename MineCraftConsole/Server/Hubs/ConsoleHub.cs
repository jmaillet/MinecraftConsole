using Microsoft.AspNetCore.SignalR;
using MineCraftConsole.Server.Services;

namespace MineCraftConsole.Server.Hubs
{
    public class ConsoleHub : Hub
    {
        private readonly IReceiver _receiver;

        public ConsoleHub(IReceiver receiver)
        {
            _receiver = receiver;
        }

        public Task SendCommandAsync(string command)
        {
            Console.WriteLine($"Hub received command: {command}");
            return _receiver.Receive(command);
        }
    }
}
