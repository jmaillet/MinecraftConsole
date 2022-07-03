namespace MineCraftConsole.Shared;

public class ServerInfo
{
  public ServerInfo(string id, ReleaseType releaseType, string url, DateTimeOffset time, DateTimeOffset releaseTime)
  {
    Id = id;
    ReleaseType = releaseType;
    Url = url;
    Time = time;
    ReleaseTime = releaseTime;
  }

  public string Id { get; init; }

  public ReleaseType ReleaseType { get; init; }

  public string Url { get; init; }

  public DateTimeOffset Time { get; init; }

  public DateTimeOffset ReleaseTime { get; init; }

}
