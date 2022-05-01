namespace MineCraftConsole.Shared;
public interface IConsoleClient
{
  Task ReceiveLine(string message);
}
