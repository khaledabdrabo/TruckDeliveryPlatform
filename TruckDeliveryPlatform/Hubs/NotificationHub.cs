using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace TruckDeliveryPlatform.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string userId, string title, string message, string link)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", new
            {
                title = title,
                message = message,
                link = link,
                timestamp = DateTime.UtcNow
            });
        }

        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
    }
} 