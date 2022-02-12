
namespace MineCraftConsole.Server.Services
{
  public interface ICoordinator
  {
    Task StartServerAsync(CancellationToken token);
   
  }
}