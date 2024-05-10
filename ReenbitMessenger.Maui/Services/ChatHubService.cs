using Microsoft.AspNetCore.SignalR.Client;
using ReenbitMessenger.Infrastructure.Models.Requests;

namespace ReenbitMessenger.Maui.Services
{
    public class ChatHubService : IDisposable
    {
        private HubConnection? _hubConnection;
        private Dictionary<string, Delegate> _notificationHandlers = new Dictionary<string, Delegate>();
        private List<string> _connectedChats = new List<string>();

        public async Task InitializeAsync(string accessToken)
        {
            if(_hubConnection is null)
            {
                _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://rb-messenger.azurewebsites.net" + "/chathub", options => {
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
            if(_notificationHandlers is null)
            {
                return;
            }

            foreach(var pair in _notificationHandlers)
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

        public async Task ConnectToGroupChatAsync(string chatId)
        {
            if (!_connectedChats.Contains(chatId))
            {
                await _hubConnection.SendAsync("ConnectToGroupChat", chatId);
                _connectedChats.Add(chatId);
            }
        }

        public async Task DisconnectFromGroupChatAsync(string chatId)
        {
            if (_connectedChats.Contains(chatId))
            {
                await _hubConnection.SendAsync("DisconnectFromGroupChat", chatId);
                _connectedChats.Remove(chatId);
            }
        }

        public async Task GetGroupChatInfoAsync(string chatId)
        {
            await _hubConnection.SendAsync("GetGroupChatInfo", chatId);
        }

        public async Task GetGroupChatMessages(string chatId,
            int page = 0, int numberOfMessages = 20, string messageContains = "", bool ascending = true)
        {
            await _hubConnection.SendAsync("GetGroupChatMessages", chatId,
                page, numberOfMessages, messageContains, ascending);
        }

        public async Task SendMessageAsync(string chatId, SendMessageToGroupChatRequest sendMessageRequest)
        {
            await _hubConnection.SendAsync("SendGroupChatMessage", chatId, sendMessageRequest);
        }

        public async Task DeleteMessageAsync(string chatId, DeleteMessageFromGroupChatRequest deleteMessageRequest)
        {
            await _hubConnection.SendAsync("DeleteGroupChatMessage", chatId, deleteMessageRequest);
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
                Task.FromResult(UnsubscribeAllAsync());
                Task.FromResult(_hubConnection.DisposeAsync());
            }
        }
    }
}
