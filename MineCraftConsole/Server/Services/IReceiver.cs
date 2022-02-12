using CliWrap;

namespace MineCraftConsole.Server.Services
{
    public interface IReceiver
    {
        Task Receive(string text);
    }
}