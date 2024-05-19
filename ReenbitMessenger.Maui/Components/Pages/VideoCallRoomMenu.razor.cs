using Microsoft.JSInterop;
using System.Reflection;

namespace ReenbitMessenger.Maui.Components.Pages
{
    public partial class VideoCallRoomMenu
    {
        private async Task JoinRoom()
        {
            if (string.IsNullOrEmpty(_roomId))
            {
                return;
            }

            navManager.NavigateTo(navManager.Uri + "/" + _roomId, true);
        }

        protected override async Task OnInitializedAsync()
        {
            await Setup();
        }

        private async Task CreateRoom()
        {
            await callService.CreateRoomAsync();
        }

        private void OnRoomCreated(string roomId)
        {
            _roomId = roomId;
            JoinRoom().GetAwaiter().GetResult();
        }

        private async Task Setup()
        {
            var token = await localStorage.GetItemAsStringAsync("jwt");

            await callService.InitializeAsync(token);

            await callService.SubscribeAsync<string>("ReceiveNewRoomId", OnRoomCreated);

            await callService.StartAsync();
        }
    }
}
