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
            var viewModel = new AdminDashboardViewModel
            {
                TotalCustomers = await _context.Users.Where(u => u.UserType == UserType.Customer).ToListAsync(),
                TotalTruckOwners = await _context.Users.Where(u => u.UserType == UserType.TruckOwner).ToListAsync(),
                ActiveJobs = await _context.Jobs.CountAsync(j => j.Status == JobStatus.Active),
                TotalBids = await _context.Bids.CountAsync(),
                RecentJobs = await _context.Jobs
                    .Include(j => j.Customer)
                    .OrderByDescending(j => j.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                RecentTruckOwners = await _context.TruckOwnerProfiles
                    .Include(p => p.User)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            // Get or create system config
            var systemConfig = await _context.SystemConfigs.FirstOrDefaultAsync();
            if (systemConfig == null)
            {
                systemConfig = new SystemConfig
                {
                    PricePerKilometer = 2.5M, // Default value
                    BaseFee = 50M, // Default value
                    CreatedAt = DateTime.UtcNow
                };
                _context.SystemConfigs.Add(systemConfig);
                await _context.SaveChangesAsync();
            }
            viewModel.SystemConfig = systemConfig;

            // Calculate statistics
            var allJobs = await _context.Jobs.ToListAsync();
            viewModel.TotalJobs = allJobs.Count;
            viewModel.CompletedJobs = allJobs.Count(j => j.Status == JobStatus.Completed);
            viewModel.CompletionRate = viewModel.TotalJobs > 0 
                ? (double)viewModel.CompletedJobs / viewModel.TotalJobs * 100 
                : 0;

            // Calculate jobs by status
            viewModel.JobsByStatus = allJobs
                .GroupBy(j => j.Status)
                .ToDictionary(g => g.Key, g => g.Count());

            // Ensure all statuses are represented
            foreach (JobStatus status in Enum.GetValues(typeof(JobStatus)))
            {
                if (!viewModel.JobsByStatus.ContainsKey(status))
                {
                    viewModel.JobsByStatus[status] = 0;
                }
            }

            // Calculate average bids per job
            viewModel.AverageBidsPerJob = viewModel.TotalJobs > 0 
                ? (double)viewModel.TotalBids / viewModel.TotalJobs 
                : 0;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSystemConfig(decimal pricePerKilometer, decimal baseFee)
        {
            try
            {
                var config = await _context.SystemConfigs.FirstOrDefaultAsync();
                if (config == null)
                {
                    config = new SystemConfig
                    {
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.SystemConfigs.Add(config);
                }

                config.PricePerKilometer = pricePerKilometer;
                config.BaseFee = baseFee;
                config.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "System configuration updated successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating system configuration: {ex.Message}";
            }

            return RedirectToAction(nameof(Dashboard));
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