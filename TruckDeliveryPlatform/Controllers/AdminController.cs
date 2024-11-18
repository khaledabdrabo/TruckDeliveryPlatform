using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckDeliveryPlatform.Data;
using TruckDeliveryPlatform.Models;
using TruckDeliveryPlatform.Models.ViewModels;

namespace TruckDeliveryPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var jobsWithBids = await _context.Jobs
                .Include(j => j.Bids)
                .Where(j => j.Bids.Any())
                .ToListAsync();

            double averageBidsPerJob = jobsWithBids.Any() 
                ? jobsWithBids.Average(j => j.Bids.Count) 
                : 0;

            var stats = new AdminDashboardViewModel
            {
                TotalCustomers = await _userManager.GetUsersInRoleAsync("Customer"),
                TotalTruckOwners = await _userManager.GetUsersInRoleAsync("TruckOwner"),
                TotalJobs = await _context.Jobs.CountAsync(),
                TotalBids = await _context.Bids.CountAsync(),
                ActiveJobs = await _context.Jobs.CountAsync(j => j.Status == JobStatus.Active),
                CompletedJobs = await _context.Jobs.CountAsync(j => j.Status == JobStatus.Completed),
                SystemConfig = await _context.SystemConfigs.FirstAsync(),
                RecentJobs = await _context.Jobs
                    .Include(j => j.Customer)
                    .OrderByDescending(j => j.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                RecentTruckOwners = await _context.TruckOwnerProfiles
                    .Include(p => p.User)
                    .OrderByDescending(p => p.Id)
                    .Take(5)
                    .ToListAsync(),
                JobsByStatus = await _context.Jobs
                    .GroupBy(j => j.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Status, x => x.Count),
                AverageBidsPerJob = averageBidsPerJob
            };

            return View(stats);
        }

        public async Task<IActionResult> TruckOwners()
        {
            var truckOwners = await _context.TruckOwnerProfiles
                .Include(p => p.User)
                .Include(p => p.TruckType)
                .OrderByDescending(p => p.AverageRating)
                .ToListAsync();

            return View(truckOwners);
        }

        public async Task<IActionResult> SystemConfig()
        {
            var config = await _context.SystemConfigs.FirstAsync();
            return View(config);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSystemConfig(SystemConfig model)
        {
            if (ModelState.IsValid)
            {
                var config = await _context.SystemConfigs.FirstAsync();
                config.PricePerKilometer = model.PricePerKilometer;
                config.BaseFee = model.BaseFee;
                await _context.SaveChangesAsync();
                TempData["Message"] = "System configuration updated successfully.";
                return RedirectToAction(nameof(SystemConfig));
            }
            return View("SystemConfig", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleTruckOwnerStatus(string userId)
        {
            var profile = await _context.TruckOwnerProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                return NotFound();

            profile.IsActive = !profile.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(TruckOwners));
        }
    }
} 