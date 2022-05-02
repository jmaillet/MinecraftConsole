using Microsoft.AspNetCore.Components;
using MineCraftConsole.Shared;
using System.Net.Http.Json;
namespace MineCraftConsole.Client.Pages;

public partial class Servers
{
  [Inject]
  private HttpClient HttpClient { get; set; } = default!;

  private ServerInstance[]? _servers;

  protected override async Task OnInitializedAsync()
  {
    _servers = await HttpClient.GetFromJsonAsync<ServerInstance[]>("api/servers");
  }
}
