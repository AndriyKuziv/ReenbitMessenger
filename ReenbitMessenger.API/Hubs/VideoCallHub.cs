using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ReenbitMessenger.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class VideoCallHub : Hub
    {
        public async Task CreateRoom()
        {
            var roomId = Convert.ToString(new Guid());

            await Clients.Caller.SendAsync("ReceiveRoomId", roomId);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.Group(roomId).SendAsync("ReceiveJoinedUser", Context.ConnectionId);
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("RemoveLeavingUser", Context.ConnectionId);
        }
    }
}
