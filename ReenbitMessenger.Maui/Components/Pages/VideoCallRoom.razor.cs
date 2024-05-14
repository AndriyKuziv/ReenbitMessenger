using Microsoft.JSInterop;

namespace ReenbitMessenger.Maui.Components.Pages
{
    public partial class VideoCallRoom
    {
        private IJSObjectReference _module;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module = await Js.InvokeAsync<IJSObjectReference>("import", "./Components/Pages/VideoCallRoom.razor.js");
                await Setup();
                await callService.JoinRoomAsync(RoomId);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {

        }

        private async Task Setup()
        {
            var token = await localStorage.GetItemAsStringAsync("jwt");
            await callService.InitializeAsync(token);
            await callService.SubscribeAsync<string>("ReceiveJoinedUser", OnUserJoined);
            await callService.SubscribeAsync<string>("RemoveLeavingUser", OnUserLeaving);

            await callService.StartAsync();

            await _module.InvokeVoidAsync("Start", token, RoomId);
        }

        private void OnUserJoined(string userConnectionId)
        {
            InvokeAsync(async () =>
            {
                await _module.InvokeVoidAsync("AddJoinedUser", userConnectionId);

                StateHasChanged();
            });
        }

        private void OnUserLeaving(string userConnectionId)
        {
            InvokeAsync(async () =>
            {
                await _module.InvokeVoidAsync("RemoveLeavingUser", userConnectionId);

                StateHasChanged();
            });
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
