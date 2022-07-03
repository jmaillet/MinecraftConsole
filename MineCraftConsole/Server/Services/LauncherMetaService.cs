using MineCraftConsole.Shared;
using System.Text.Json;

namespace MineCraftConsole.Server.Services;

public class LauncherMetaService
{
  private readonly IHttpClientFactory _httpClientFactory;

  public LauncherMetaService(IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory;
  }

  private IEnumerable<ServerInfo> ReadVersionManifest()
  {
    var stream = new FileStream(".\\VersionMeta\\version_manifest.json", FileMode.Open);
    var manifest = JsonSerializer.Deserialize<Manifest>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    return manifest!.Versions.Where(si => si.ReleaseType == ReleaseType.Release);
  }

  private async Task<ServerVersion> DownloadPackage(ServerInfo info)
  {
    var client = _httpClientFactory.CreateClient();
    using var stream = await client.GetStreamAsync(info.Url);
    var doc = await JsonDocument.ParseAsync(stream);

    var root = doc.RootElement;

    var server = root.GetProperty("downloads").GetProperty("server");
    return new ServerVersion(
      root.GetProperty("id").ToString(),
      server.GetProperty("sha1").ToString(),
      server.GetProperty("size").GetInt32(),
      server.GetProperty("url").ToString()
    );
  }

  public async Task<IEnumerable<ServerVersion>> GetServerVersions()
  {
    var infos = ReadVersionManifest();
    return await Task.WhenAll(infos.Select(DownloadPackage));
  }

  //[JsonConstructor]
  private record CurrentVersion(string Snapshot, string Release);
  private record Manifest(CurrentVersion Latest, ServerInfo[] Versions);
}
