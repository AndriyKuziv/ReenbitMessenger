using Microsoft.AspNetCore.SignalR.Client;
using ReenbitMessenger.Infrastructure.Models.Requests;

namespace ReenbitMessenger.Maui.Services
{
    public class ChatHubService : IDisposable
    {
        private static HubConnection? _hubConnection;
        private Dictionary<string, Delegate> _notificationHandlers = new Dictionary<string, Delegate>();

        public async Task InitializeAsync(string accessToken)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7051" + "/chathub", options => {
                    options.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .Build();
        }

        public async Task StartAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StartAsync();
            }
        }

        public async Task StopAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
            }
        }

        public async Task SubscribeAsync<T>(string notificationName, Action<T> handler)
        {
            if (!_notificationHandlers.ContainsKey(notificationName))
            {
                _notificationHandlers[notificationName] = handler;
            }
            else
            {
                _notificationHandlers[notificationName] = Delegate.Combine(_notificationHandlers[notificationName], handler);
            }

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
            if (!_notificationHandlers.ContainsKey(notificationName))
            {
                return;
            }

            Delegate.Remove(_notificationHandlers[notificationName], handler);
            _notificationHandlers.Remove(notificationName);
        }

        private void HandleMessage<T>(string notificationName, T notificationObject)
        {
            if (_notificationHandlers.ContainsKey(notificationName))
            {
                ((Action<T>)_notificationHandlers[notificationName])?.Invoke(notificationObject);
            }
        }


        public async Task ConnectToGroupChatAsync(string chatId)
        {
            await _hubConnection.SendAsync("ConnectToGroupChat", chatId);
        }

        public async Task SendMessageAsync(string chatId, SendMessageToGroupChatRequest sendMessageRequest)
        {
            await _hubConnection.SendAsync("SendGroupChatMessage", chatId, sendMessageRequest);
        }

        public async Task AddUsersToGroupChatAsync(string chatId, AddUsersToGroupChatRequest addUsersRequest)
        {
            await _hubConnection.SendAsync("AddUsersToGroupChat", chatId, addUsersRequest);
        }

        public async Task RemoveUsersFromGroupChatAsync(string chatId, RemoveUsersFromGroupChatRequest removeUsersRequest)
        {
            await _hubConnection.SendAsync("RemoveUsersFromGroupChat", chatId, removeUsersRequest);
        }

        public void Dispose()
        {
            try {
                Task.FromResult(_hubConnection.StopAsync());
            }
            finally
            {
                Task.FromResult(_hubConnection.DisposeAsync());
            }
        }
    }
}
