namespace MineCraftConsole.Shared;

public class ServerVersion
{

  public ServerVersion(string id, string sha1, int size, string url)
  {
    Id = id;
    Sha1 = sha1;
    Size = size;
    Url = url;
  }

  public string Id { get; set; }

  public string Sha1 { get; set; }

  public int Size { get; set; }

  public string Url { get; set; }
}
