using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Channels;

namespace MineCraftConsole.Client.Pages;

public partial class Index
{
  private HubConnection? _hubConnection = default!;
  private List<string> _messages = new List<string>();
  private string _command = string.Empty;
  private ChannelReader<string> _consoleReader = default!;
  private Channel<string> _channel = default!;

  protected override async Task OnInitializedAsync()
  {
    _channel = Channel.CreateBounded<string>(100);

   
    _hubConnection = new HubConnectionBuilder()
      .WithUrl(NavigationManager
      .ToAbsoluteUri("/consolehub"))
      .Build();

    await _hubConnection.StartAsync();

    _consoleReader = await _hubConnection.StreamAsChannelAsync<string>("ReadMessageStream");
    await _hubConnection.SendAsync("WriteMessageStream", _channel.Reader);

  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender)
    {
      await foreach(var message in _consoleReader.ReadAllAsync())
      {
        _messages.Add(message);
        StateHasChanged();
      }
    }
  }

  private async Task OnEnter(KeyboardEventArgs e)
  {
    if (e.Key == "Enter")
    {
      await _channel.Writer.WriteAsync(_command);
      _command = "";
    }
  }
  
  public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
  public async ValueTask DisposeAsync()
  {
    if (_hubConnection is not null)
    {
      await _hubConnection.DisposeAsync();
    }
  }
}
