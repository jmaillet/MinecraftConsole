using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using MineCraftConsole.Shared;

namespace MineCraftConsole.Client.Pages;

public partial class Index
{
  [Inject]
  private HttpClient HttpClient { get; set; } = default!;

  private HubConnection _hubConnection = default!; // set in OnInit
  private List<string> _messages = new();
  private string _command = string.Empty;


  protected async override Task OnInitializedAsync()
  {
    _hubConnection = new HubConnectionBuilder()
      .WithUrl(NavigationManager
      .ToAbsoluteUri("/consolehub"))
      .Build();

    _hubConnection.On<string>(nameof(IConsoleClient.ReceiveLine), RecieveLine);

    await _hubConnection.StartAsync();
  }

  private async Task StartServer()
  {
    var resp = await HttpClient.PostAsync("/api/server/start",null);
    _messages.Add($"HTTP {resp.StatusCode}");
    _messages.Add(await resp.Content.ReadAsStringAsync());
  }

  private async Task StopServer()
  {
    await HttpClient.PostAsync("/api/server/stop", null);
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
