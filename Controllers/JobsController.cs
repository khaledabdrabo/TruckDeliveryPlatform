using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TruckDeliveryPlatform.Data;
using TruckDeliveryPlatform.Models;
using Microsoft.AspNetCore.Identity;

namespace TruckDeliveryPlatform.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var jobs = await _context.Jobs
                .Where(j => j.CustomerId == currentUser.Id)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
            
            return View(jobs);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                
                var job = new Job
                {
                    CustomerId = currentUser.Id,
                    PickupLocation = model.PickupLocation,
                    DropoffLocation = model.DropoffLocation,
                    PickupLatitude = model.PickupLatitude,
                    PickupLongitude = model.PickupLongitude,
                    DropoffLatitude = model.DropoffLatitude,
                    DropoffLongitude = model.DropoffLongitude,
                    GoodsType = model.GoodsType,
                    Quantity = model.Quantity,
                    Weight = model.Weight,
                    Size = model.Size,
                    PreferredPickupDate = model.PreferredPickupDate,
                    SpecialInstructions = model.SpecialInstructions,
                    EstimatedDistance = model.EstimatedDistance,
                    EstimatedCost = model.EstimatedCost,
                    Status = JobStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
                .Include(j => j.Bids)
                .ThenInclude(b => b.TruckOwner)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }
    }
} 