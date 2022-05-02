using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using MineCraftConsole.Shared;
using System.Net.Http.Json;

namespace MineCraftConsole.Client.Pages;

public partial class ServerConsole
{
  [Inject]
  private HttpClient HttpClient { get; set; } = default!;

  [Parameter]
  public int Id { get; set; }

  private HubConnection _hubConnection = default!; // set in OnInit
  private List<string> _messages = new();
  private string _command = string.Empty;

  private bool IsRunning => _server?.ProcessId is not null;
  private ServerInstance? _server = null;

  protected async override Task OnInitializedAsync()
  {
    _hubConnection = new HubConnectionBuilder()
      .WithUrl(NavigationManager
      .ToAbsoluteUri("/consolehub"))
      .Build();

    _hubConnection.On<string>(nameof(IConsoleClient.ReceiveLine), RecieveLine);

    await _hubConnection.StartAsync();
    _server = await HttpClient.GetFromJsonAsync<ServerInstance>($"api/servers/{Id}");
  }

  private async Task StartServer()
  {
    var resp = await HttpClient.PostAsync($"/api/servers/{Id}/start", null);
    _messages.Add($"HTTP {resp.StatusCode}");
    if (resp.IsSuccessStatusCode)
    {
      var procId = await resp.Content.ReadAsStringAsync();
      _messages.Add(procId);
      _server!.ProcessId = int.Parse(procId);
    }
  }

  private async Task StopServer()
  {
    var resp = await HttpClient.PostAsync($"/api/servers/{Id}/stop", null);
    if (resp.IsSuccessStatusCode)
    {
      _server!.ProcessId = null;
    }
  }

  private void RecieveLine(string line)
  {
    _messages.Add(line);
    StateHasChanged();
  }

  private async Task OnEnter(KeyboardEventArgs e)
  {
    if (e.Key == "Enter")
    {
      await _hubConnection.SendAsync("Send", _command);
    }
  }

  public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
}
