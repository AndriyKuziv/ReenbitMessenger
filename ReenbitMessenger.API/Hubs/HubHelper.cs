using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ReenbitMessenger.API.Hubs
{
    public static class HubHelper
    {
        public static async Task<string> GetUserIdAsync(HubCallerContext context)
        {
            if (context.User is null)
            {
                return null;
            }

            var identity = context.User.Identity as ClaimsIdentity;
            if (identity is null)
            {
                return null;
            }

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            return userId;
        }
    }
}
