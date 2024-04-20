using Blazored.LocalStorage;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.FileIO;
using ReenbitMessenger.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Maui.Services
{
    public class ChatHubService
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
    }
}
