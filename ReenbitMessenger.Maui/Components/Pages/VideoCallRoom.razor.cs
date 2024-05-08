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
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            await Setup();
            await callService.JoinRoomAsync(CallId);
        }

        private async Task Setup()
        {
            await callService.InitializeAsync(await localStorage.GetItemAsStringAsync("jwt"));
            await callService.SubscribeAsync<string>("ReceiveJoinedUser", OnUserJoined);
            await callService.SubscribeAsync<string>("RemoveLeavingUser", OnUserLeaving);

            await callService.StartAsync();
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
            await callService.LeaveRoomAsync(CallId);
            await callService.UnsubscribeAllAsync();

            callService.Dispose();
        }
    }
}
