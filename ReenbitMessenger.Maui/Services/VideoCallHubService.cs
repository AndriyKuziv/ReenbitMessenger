using Microsoft.AspNetCore.SignalR.Client;

namespace ReenbitMessenger.Maui.Services
{
    public class VideoCallHubService : IDisposable
    {
        private HubConnection? _hubConnection;
        private Dictionary<string, Delegate> _notificationHandlers = new Dictionary<string, Delegate>();

        public async Task InitializeAsync(string accessToken)
        {
            if (_hubConnection is null)
            {
                _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7051" + "/callhub", options => {
                    options.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .Build();
            }
        }

        public async Task StartAsync()
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();
            }
        }

        public async Task StopAsync()
        {
            if (_hubConnection != null && _hubConnection.State != HubConnectionState.Disconnected)
            {
                await _hubConnection.StopAsync();
            }
        }

        public async Task JoinRoomAsync(string roomId)
        {
            await _hubConnection.InvokeAsync("JoinRoom", roomId);
        }

        public async Task CreateRoomAsync()
        {
            await _hubConnection.InvokeAsync("CreateRoom");
        }

        public async Task LeaveRoomAsync(string roomId)
        {
            await _hubConnection.InvokeAsync("LeaveRoom", roomId);
        }

        public async Task SubscribeAsync<T>(string notificationName, Action<T> handler)
        {
            if (_notificationHandlers is null || _notificationHandlers.ContainsKey(notificationName))
            {
                return;
            }

            _notificationHandlers[notificationName] = handler;

            if (_hubConnection != null)
            {
                _hubConnection.On<T>(notificationName, (message) =>
                {
                    HandleMessage(notificationName, message);
                });
            }
        }

        public async Task UnsubscribeAsync<T>(string notificationName, Action<T> handler)
        {
            if (_notificationHandlers != null && !_notificationHandlers.ContainsKey(notificationName))
            {
                return;
            }

            Delegate.Remove(_notificationHandlers[notificationName], handler);
            _notificationHandlers.Remove(notificationName);
        }

        public async Task UnsubscribeAllAsync()
        {
            if (_notificationHandlers is null)
            {
                return;
            }

            foreach (var pair in _notificationHandlers)
            {
                _hubConnection.Remove(pair.Key);
                Delegate.Remove(_notificationHandlers[pair.Key], pair.Value);
                _notificationHandlers.Remove(pair.Key);
            }
        }

        private void HandleMessage<T>(string notificationName, T notificationObject)
        {
            if (_notificationHandlers != null && _notificationHandlers.ContainsKey(notificationName))
            {
                ((Action<T>)_notificationHandlers[notificationName])?.Invoke(notificationObject);
            }
        }

        public void Dispose()
        {
            try
            {
                Task.FromResult(_hubConnection.StopAsync());
            }
            finally
            {
                Task.FromResult(UnsubscribeAllAsync());
                Task.FromResult(_hubConnection.DisposeAsync());
            }
        }
    }
}
