using Microsoft.JSInterop;

namespace ReenbitMessenger.Maui.Components.Pages
{
    public partial class VideoCallRoom
    {
        private IJSObjectReference _module;

        protected override async Task OnInitializedAsync()
        {
            _module = await Js.InvokeAsync<IJSObjectReference>("import", "./Components/Pages/VideoCallRoom.razor.js");
            await Setup();
            await callService.JoinRoomAsync(RoomId);
            await base.OnInitializedAsync();
        }

        private async Task Setup()
        {
            var token = await localStorage.GetItemAsStringAsync("jwt");
            await callService.InitializeAsync(token);

            await callService.StartAsync();

            await _module.InvokeVoidAsync("Start", token, RoomId);
        }

        public async ValueTask DisposeAsync()
        {
            await _module.InvokeVoidAsync("End");

            await callService.LeaveRoomAsync(RoomId);
            await callService.UnsubscribeAllAsync();

            callService.Dispose();
        }
    }
}
