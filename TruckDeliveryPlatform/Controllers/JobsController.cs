using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckDeliveryPlatform.Data;
using TruckDeliveryPlatform.Models;
using TruckDeliveryPlatform.Models.ViewModels;
using TruckDeliveryPlatform.Services;
using TruckDeliveryPlatform.Helpers;

namespace TruckDeliveryPlatform.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Jobs
        public async Task<IActionResult> Index(int? page)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var pageNumber = page ?? 1;
            const int pageSize = 10;

            var jobs = _context.Jobs
                .Include(j => j.Bids)
                .Where(j => j.CustomerId == currentUser.Id)
                .OrderByDescending(j => j.CreatedAt);

            var paginatedJobs = await PaginatedList<Job>.CreateAsync(jobs, pageNumber, pageSize);
            
            return View(paginatedJobs);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (currentUser.UserType != UserType.Customer)
            {
                return RedirectToAction("Dashboard", "TruckOwner");
            }

            ViewBag.Locations = _context.Locations
                .OrderBy(l => l.State)
                .ThenBy(l => l.City)
                .ThenBy(l => l.Name)
                .ToList();

            ViewBag.TruckTypes = _context.TruckTypes.OrderBy(t => t.Name).ToList();

            return View(new JobViewModel());
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser.UserType != UserType.Customer)
                {
                    return RedirectToAction("Dashboard", "TruckOwner");
                }

                var pickupLocation = await _context.Locations.FindAsync(model.PickupLocationId);
                var dropoffLocation = await _context.Locations.FindAsync(model.DropoffLocationId);
                var config = await _context.SystemConfigs.FirstAsync();

                if (pickupLocation == null || dropoffLocation == null)
                {
                    ModelState.AddModelError("", "Invalid location selected");
                    ViewBag.Locations = _context.Locations.OrderBy(l => l.State).ThenBy(l => l.City).ThenBy(l => l.Name).ToList();
                    ViewBag.TruckTypes = _context.TruckTypes.OrderBy(t => t.Name).ToList();
                    return View(model);
                }

                // Calculate distance and cost
                var distance = DistanceCalculator.CalculateDistance(pickupLocation, dropoffLocation);
                var estimatedCost = (decimal)distance * config.PricePerKilometer + config.BaseFee;

                var job = new Job
                {
                    CustomerId = currentUser.Id,
                    PickupLocationId = model.PickupLocationId,
                    DropoffLocationId = model.DropoffLocationId,
                    PickupLocation = pickupLocation.FullAddress,
                    DropoffLocation = dropoffLocation.FullAddress,
                    GoodsType = model.GoodsType,
                    Quantity = model.Quantity,
                    Weight = model.Weight,
                    Size = model.Size,
                    PreferredPickupDate = model.PreferredPickupDate,
                    SpecialInstructions = model.SpecialInstructions,
                    TruckTypeId = model.TruckTypeId,
                    EstimatedCost = estimatedCost,
                    Status = JobStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    AcceptedBidId = null,
                    CancelledAt = null
                };

                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "Customer");
            }

            ViewBag.Locations = _context.Locations
                .OrderBy(l => l.State)
                .ThenBy(l => l.City)
                .ThenBy(l => l.Name)
                .ToList();
            ViewBag.TruckTypes = _context.TruckTypes.OrderBy(t => t.Name).ToList();

            return View(model);
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Customer)
                .Include(j => j.Bids)
                    .ThenInclude(b => b.TruckOwner)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.UserType == UserType.Customer && job.CustomerId != currentUser.Id)
            {
                return Forbid();
            }

            ViewBag.TruckOwnerProfile = null;
            if (currentUser.UserType == UserType.TruckOwner)
            {
                ViewBag.TruckOwnerProfile = await _context.TruckOwnerProfiles
                    .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);
            }

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectBid(int bidId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var bid = await _context.Bids
                .Include(b => b.Job)
                    .ThenInclude(j => j.Bids)
                .FirstOrDefaultAsync(b => b.Id == bidId);

            if (bid == null)
                return NotFound();

            if (bid.Job.CustomerId != currentUser.Id)
                return Forbid();

            if (bid.Job.Status != JobStatus.Active || bid.Job.HasAcceptedBid)
                return BadRequest("This job is not available for accepting bids");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Mark the selected bid as Selected (waiting for confirmation)
                bid.Status = BidStatus.Selected;
                
                // Mark all other bids as rejected
                foreach (var otherBid in bid.Job.Bids.Where(b => b.Id != bidId))
                {
                    otherBid.Status = BidStatus.Rejected;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Send notification to truck owner (you can implement this later)
                // await _notificationService.NotifyTruckOwner(bid.TruckOwnerId, "A customer has selected your bid");

                return RedirectToAction(nameof(Details), new { id = bid.JobId });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBid(int bidId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var bid = await _context.Bids
                .Include(b => b.Job)
                    .ThenInclude(j => j.Bids)
                .FirstOrDefaultAsync(b => b.Id == bidId);

            if (bid == null)
                return NotFound();

            if (bid.TruckOwnerId != currentUser.Id)
                return Forbid();

            if (bid.Status != BidStatus.Selected)
                return BadRequest("This bid is not awaiting confirmation");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Accept the bid and update job status
                bid.Status = BidStatus.Accepted;
                bid.Job.Status = JobStatus.Accepted;
                bid.Job.AcceptedBidId = bid.Id;
                bid.Job.AcceptedAt = DateTime.UtcNow;
                bid.Job.AcceptedBidAmount = bid.BidAmount;

                // Mark all other bids as rejected
                foreach (var otherBid in bid.Job.Bids.Where(b => b.Id != bidId))
                {
                    otherBid.Status = BidStatus.Rejected;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Details), new { id = bid.JobId });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineBid(int bidId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var bid = await _context.Bids
                .Include(b => b.Job)
                .FirstOrDefaultAsync(b => b.Id == bidId);

            if (bid == null)
                return NotFound();

            if (bid.TruckOwnerId != currentUser.Id)
                return Forbid();

            if (bid.Status != BidStatus.Selected)
                return BadRequest("This bid is not awaiting confirmation");

            // Mark the bid as rejected
            bid.Status = BidStatus.Rejected;
            
            // Reopen the job for new bids
            bid.Job.AcceptedBidId = null;
            bid.Job.AcceptedAt = null;
            bid.Job.AcceptedBidAmount = null;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = bid.JobId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReopenJob(int jobId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var job = await _context.Jobs.FindAsync(jobId);

            if (job == null || job.CustomerId != currentUser.Id)
                return NotFound();

            if (job.Status != JobStatus.Active)
                return BadRequest("Only active jobs can be reopened");

            // Clear accepted bid and reopen for bidding
            job.AcceptedBidId = null;
            job.AcceptedAt = null;
            job.AcceptedBidAmount = null;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = jobId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseJob(int jobId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var job = await _context.Jobs.FindAsync(jobId);

            if (job == null || job.CustomerId != currentUser.Id)
                return NotFound();

            if (job.Status != JobStatus.Active)
                return BadRequest("Only active jobs can be closed");

            job.Status = JobStatus.Canceled;
            job.CancelledAt = DateTime.UtcNow;
            job.CancellationReason = "Closed by customer - No suitable bids";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = jobId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateTruckOwner(RatingViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var job = await _context.Jobs
                .Include(j => j.AcceptedBid)
                .ThenInclude(b => b.TruckOwner)
                .FirstOrDefaultAsync(j => j.Id == model.JobId);

            if (job == null || job.CustomerId != currentUser.Id)
                return NotFound();

            if (job.Status != JobStatus.Completed)
                return BadRequest("Can only rate completed jobs");

            var truckOwnerProfile = await _context.TruckOwnerProfiles
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.Id == model.TruckOwnerProfileId);

            if (truckOwnerProfile == null)
                return NotFound();

            // Check if already rated
            if (await _context.TruckOwnerRatings.AnyAsync(r => 
                r.JobId == job.Id && r.CustomerId == currentUser.Id))
            {
                return BadRequest("You have already rated this truck owner for this job");
            }

            var rating = new TruckOwnerRating
            {
                TruckOwnerProfileId = model.TruckOwnerProfileId,
                CustomerId = currentUser.Id,
                JobId = job.Id,
                Rating = model.Rating,
                Comment = model.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.TruckOwnerRatings.Add(rating);

            // Update average rating
            truckOwnerProfile.TotalRatings++;
            var totalRatingPoints = truckOwnerProfile.Ratings.Sum(r => r.Rating) + rating.Rating;
            truckOwnerProfile.AverageRating = (decimal)totalRatingPoints / truckOwnerProfile.TotalRatings;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = job.Id });
        }
    }
} 