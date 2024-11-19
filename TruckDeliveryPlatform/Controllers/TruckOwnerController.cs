using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckDeliveryPlatform.Data;
using TruckDeliveryPlatform.Models;
using TruckDeliveryPlatform.Helpers;
using TruckDeliveryPlatform.Services;
using TruckDeliveryPlatform.Models.ViewModels;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace TruckDeliveryPlatform.Controllers
{
    [Authorize(Roles = "TruckOwner")]
    public class TruckOwnerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ImageService _imageService;
        private readonly ILogger<TruckOwnerController> _logger;

        public TruckOwnerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ImageService imageService, ILogger<TruckOwnerController> logger)
        {
            _context = context;
            _userManager = userManager;
            _imageService = imageService;
            _logger = logger;
        }

        public async Task<IActionResult> Dashboard(int? page)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user.UserType != UserType.TruckOwner)
            {
                return RedirectToAction("Dashboard", "Customer");
            }

            // Check if profile is complete
            var profile = await _context.TruckOwnerProfiles
                .Include(p => p.ServiceAreas)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                return RedirectToAction("CreateProfile");
            }

            var pageNumber = page ?? 1;
            const int pageSize = 10;

            // Get jobs in service areas only
            var serviceAreaIds = profile.ServiceAreas.Select(a => a.Id).ToList();
            
            // Debug: Log the values
            _logger.LogInformation($"Service Areas: {string.Join(", ", serviceAreaIds)}");
            _logger.LogInformation($"Truck Type: {profile.TruckTypeId}");

            var availableJobs = _context.Jobs
                .Include(j => j.Customer)
                .Include(j => j.Bids)
                .Where(j => j.Status == JobStatus.Active &&  // Only active jobs
                           !j.AcceptedBidId.HasValue &&      // No accepted bid
                           !j.CancelledAt.HasValue &&        // Not cancelled
                           j.TruckTypeId == profile.TruckTypeId && // Matching truck type
                           (serviceAreaIds.Contains(j.PickupLocationId) || 
                            serviceAreaIds.Contains(j.DropoffLocationId))) // In service area
                .OrderByDescending(j => j.CreatedAt);

            // Debug: Log the count before pagination
            var totalCount = await availableJobs.CountAsync();
            _logger.LogInformation($"Found {totalCount} available jobs before pagination");

            var paginatedJobs = await PaginatedList<Job>.CreateAsync(availableJobs, pageNumber, pageSize);

            return View(paginatedJobs);
        }

        [Authorize(Roles = "TruckOwner")]
        [HttpGet]
        public async Task<IActionResult> CreateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            
            // Check if profile already exists
            var existingProfile = await _context.TruckOwnerProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);
            
            if (existingProfile != null)
            {
                return RedirectToAction("Dashboard");
            }

            // Populate ViewBag with truck types and locations
            ViewBag.TruckTypes = await _context.TruckTypes.OrderBy(t => t.Name).ToListAsync();
            ViewBag.Locations = await _context.Locations
                .OrderBy(l => l.State)
                .ThenBy(l => l.City)
                .ToListAsync();

            // Initialize view model with default price per kilometer
            var model = new TruckOwnerProfileViewModel
            {
                PricePerKilometer = 2.50m // Default price
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(TruckOwnerProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    
                    // Check if profile already exists
                    var existingProfile = await _context.TruckOwnerProfiles
                        .FirstOrDefaultAsync(p => p.UserId == user.Id);
                    
                    if (existingProfile != null)
                    {
                        ModelState.AddModelError("", "Profile already exists");
                        return View(model);
                    }

                    // Get selected locations
                    var selectedAreas = await _context.Locations
                        .Where(l => model.ServiceAreaIds.Contains(l.Id))
                        .ToListAsync();

                    var profile = new TruckOwnerProfile
                    {
                        UserId = user.Id,
                        CompanyName = model.CompanyName,
                        LicenseNumber = model.LicenseNumber,
                        PhoneNumber = model.PhoneNumber,
                        TruckTypeId = model.TruckTypeId,
                        Description = model.Description,
                        PricePerKilometer = model.PricePerKilometer,
                        WaitingHourPrice = model.WaitingHourPrice,
                        ServiceAreas = selectedAreas,
                        AverageRating = 0,
                        TotalRatings = 0
                    };

                    // Handle profile image upload
                    if (model.ProfileImage != null)
                    {
                        var profileImagePath = await _imageService.SaveImageAsync(model.ProfileImage, "profiles");
                        profile.ProfileImagePath = profileImagePath;
                    }

                    // Handle truck image upload
                    if (model.TruckImage != null)
                    {
                        var truckImagePath = await _imageService.SaveImageAsync(model.TruckImage, "trucks");
                        profile.TruckImagePath = truckImagePath;
                    }

                    _context.TruckOwnerProfiles.Add(profile);
                    await _context.SaveChangesAsync();

                    // Log the saved values for debugging
                    _logger.LogInformation($"Profile created - WaitingHourPrice: {profile.WaitingHourPrice}");

                    return RedirectToAction("Dashboard");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating profile: {ex.Message}");
                    ModelState.AddModelError("", $"Error saving profile: {ex.Message}");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.TruckTypes = await _context.TruckTypes.OrderBy(t => t.Name).ToListAsync();
            ViewBag.Locations = await _context.Locations
                .OrderBy(l => l.State)
                .ThenBy(l => l.City)
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceBid(int jobId, decimal bidAmount, DateTime estimatedDeliveryTime, string notes)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _context.TruckOwnerProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                return RedirectToAction("CreateProfile");

            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound();

            var bid = new Bid
            {
                JobId = jobId,
                TruckOwnerId = userId,
                BidAmount = bidAmount,
                WaitingHourPrice = profile.WaitingHourPrice,
                EstimatedDeliveryTime = estimatedDeliveryTime,
                Notes = notes,
                Status = BidStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Bid placed successfully";
            return RedirectToAction("Details", "Jobs", new { id = jobId });
        }

        public async Task<IActionResult> Profile(string id)
        {
            var profile = await _context.TruckOwnerProfiles
                .Include(p => p.User)
                .Include(p => p.TruckType)
                .Include(p => p.ServiceAreas)
                .Include(p => p.Ratings)
                    .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(p => p.UserId == id);

            if (profile == null)
                return NotFound();

            var completedJobs = await _context.Jobs
                .Include(j => j.Customer)
                .Where(j => j.AcceptedBid.TruckOwnerId == id && j.Status == JobStatus.Completed)
                .OrderByDescending(j => j.CompletedAt)
                .Take(5)
                .ToListAsync();

            var ratingDistribution = profile.Ratings
                .GroupBy(r => r.Rating)
                .ToDictionary(g => g.Key, g => g.Count());

            var totalJobs = await _context.Jobs
                .CountAsync(j => j.AcceptedBid.TruckOwnerId == id);

            var completionRate = totalJobs > 0 
                ? (decimal)completedJobs.Count / totalJobs * 100 
                : 0;

            var averageResponseTime = await _context.Bids
                .Where(b => b.TruckOwnerId == id)
                .Select(b => (b.CreatedAt - b.Job.CreatedAt).TotalHours)
                .AverageAsync();

            var viewModel = new TruckOwnerProfileDetailsViewModel
            {
                Profile = profile,
                RecentRatings = profile.Ratings.OrderByDescending(r => r.CreatedAt).Take(5),
                CompletedJobs = completedJobs,
                RatingDistribution = ratingDistribution,
                TotalCompletedJobs = completedJobs.Count,
                CompletionRate = completionRate,
                AverageResponseTime = (decimal)averageResponseTime
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(TruckOwnerProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.TruckOwnerProfiles
                .Include(p => p.ServiceAreas)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (model.ProfileImage != null)
                {
                    if (!string.IsNullOrEmpty(profile.ProfileImagePath))
                    {
                        _imageService.DeleteImage(profile.ProfileImagePath);
                    }
                    profile.ProfileImagePath = await _imageService.SaveImageAsync(model.ProfileImage, "profiles");
                }

                if (model.TruckImage != null)
                {
                    if (!string.IsNullOrEmpty(profile.TruckImagePath))
                    {
                        _imageService.DeleteImage(profile.TruckImagePath);
                    }
                    profile.TruckImagePath = await _imageService.SaveImageAsync(model.TruckImage, "trucks");
                }

                profile.CompanyName = model.CompanyName;
                profile.LicenseNumber = model.LicenseNumber;
                profile.PhoneNumber = model.PhoneNumber;
                profile.TruckTypeId = model.TruckTypeId;
                profile.Description = model.Description;

                profile.ServiceAreas.Clear();
                var selectedAreas = await _context.Locations
                    .Where(l => model.ServiceAreaIds.Contains(l.Id))
                    .ToListAsync();
                foreach (var area in selectedAreas)
                {
                    profile.ServiceAreas.Add(area);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Profile), new { id = user.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.TruckOwnerProfiles
                .Include(p => p.ServiceAreas)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
                return NotFound();

            var viewModel = new TruckOwnerProfileViewModel
            {
                CompanyName = profile.CompanyName,
                LicenseNumber = profile.LicenseNumber,
                PhoneNumber = profile.PhoneNumber,
                TruckTypeId = profile.TruckTypeId,
                Description = profile.Description,
                ServiceAreaIds = profile.ServiceAreas.Select(a => a.Id).ToList()
            };

            ViewBag.TruckTypes = await _context.TruckTypes.OrderBy(t => t.Name).ToListAsync();
            ViewBag.Locations = await _context.Locations
                .OrderBy(l => l.State)
                .ThenBy(l => l.City)
                .ToListAsync();
            ViewBag.SelectedAreas = viewModel.ServiceAreaIds;
            ViewBag.CurrentProfileImage = profile.ProfileImagePath;
            ViewBag.CurrentTruckImage = profile.TruckImagePath;

            return View(viewModel);
        }
    }
} 