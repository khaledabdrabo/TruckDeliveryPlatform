using Microsoft.AspNetCore.SignalR;

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
    }
} 