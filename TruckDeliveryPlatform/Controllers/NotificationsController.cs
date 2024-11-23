using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TruckDeliveryPlatform.Data;

namespace TruckDeliveryPlatform.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var result = new
            {
                notifications = await _context.Notifications
                    .Where(n => n.UserId == userId)
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(10)
                    .Select(n => new
                    {
                        title = n.Title,
                        message = n.Message,
                        link = n.Link,
                        isRead = n.IsRead,
                        createdAt = n.CreatedAt
                    })
                    .ToListAsync(),
                
                totalCount = await _context.Notifications
                    .CountAsync(n => n.UserId == userId),
                    
                unreadCount = await _context.Notifications
                    .CountAsync(n => n.UserId == userId && !n.IsRead)
            };

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
} 