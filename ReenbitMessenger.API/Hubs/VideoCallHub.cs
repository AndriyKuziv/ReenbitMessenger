using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ReenbitMessenger.API.Hubs
{
    public class VideoCallHub : Hub
    {
        private Dictionary<string, string> _connectedUsers = new Dictionary<string, string>();

        public async Task CreateRoom()
        {
            var roomId = Guid.NewGuid().ToString();

            await Clients.Caller.SendAsync("ReceiveNewRoomId", roomId);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ReceiveJoinedUser", Context.ConnectionId);
        }

        public async Task JoinHub(string userId)
        {
            _connectedUsers[Context.ConnectionId] = userId;
        }

        public override async Task OnConnectedAsync()
        {
            // This method is called when a user connects to the hub
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // This method is called when a user disconnects from the hub
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("RemoveLeavingUser", Context.ConnectionId);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
