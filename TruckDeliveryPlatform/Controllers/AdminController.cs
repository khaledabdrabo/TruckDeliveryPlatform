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
        public async Task<IActionResult> UpdateSystemConfig(decimal pricePerKilometer, decimal baseFee, decimal transactionFee)
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
                config.TransactionFee = transactionFee;
                config.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Update the constant in SystemWallet to match the new fee
                SystemWallet.TRANSACTION_FEE = transactionFee;

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

        [HttpGet]
        public async Task<IActionResult> WalletManagement(string type = null, string status = null, 
            string dateRange = null, string search = null, int page = 1)
        {
            var wallet = await _context.SystemWallets
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync();

            if (wallet == null)
            {
                wallet = new SystemWallet
                {
                    Balance = 0,
                    LastUpdated = DateTime.UtcNow,
                    Transactions = new List<Transaction>()
                };
                _context.SystemWallets.Add(wallet);
                await _context.SaveChangesAsync();
            }

            // Start with all transactions
            var transactionsQuery = _context.Transactions
                .Include(t => t.Customer)
                .Include(t => t.TruckOwner)
                .Include(t => t.Job)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(type))
            {
                if (Enum.TryParse<TransactionType>(type, out var transactionType))
                {
                    transactionsQuery = transactionsQuery.Where(t => t.Type == transactionType);
                }
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<TransactionStatus>(status, out var transactionStatus))
                {
                    transactionsQuery = transactionsQuery.Where(t => t.Status == transactionStatus);
                }
            }

            if (!string.IsNullOrEmpty(dateRange))
            {
                var currentDate = DateTime.UtcNow.Date;
                switch (dateRange.ToLower())
                {
                    case "today":
                        transactionsQuery = transactionsQuery.Where(t => t.CreatedAt.Date == currentDate);
                        break;
                    case "week":
                        var weekStart = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                        transactionsQuery = transactionsQuery.Where(t => t.CreatedAt.Date >= weekStart);
                        break;
                    case "month":
                        var currentMonthStart = new DateTime(currentDate.Year, currentDate.Month, 1);
                        transactionsQuery = transactionsQuery.Where(t => t.CreatedAt.Date >= currentMonthStart);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                transactionsQuery = transactionsQuery.Where(t => 
                    t.ReferenceNumber.Contains(search) ||
                    t.Customer.FirstName.Contains(search) ||
                    t.Customer.LastName.Contains(search) ||
                    t.TruckOwner.FirstName.Contains(search) ||
                    t.TruckOwner.LastName.Contains(search));
            }

            // Order by most recent first
            transactionsQuery = transactionsQuery.OrderByDescending(t => t.CreatedAt);

            // Pagination
            const int pageSize = 10;
            var totalItems = await transactionsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var transactions = await transactionsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Calculate total income (completed customer payments)
            var totalIncome = await _context.Transactions
                .Where(t => t.Type == TransactionType.CustomerPayment && t.Status == TransactionStatus.Completed)
                .SumAsync(t => t.Amount);

            // Calculate total payouts (completed truck owner payouts)
            var totalPayout = await _context.Transactions
                .Where(t => t.Type == TransactionType.TruckOwnerPayout && t.Status == TransactionStatus.Completed)
                .SumAsync(t => t.Amount);

            // Update system wallet balance
            wallet.Balance = totalIncome - totalPayout;
            wallet.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Calculate revenue analytics
            var revenueAnalytics = new Dictionary<string, decimal>();
            
            // Total Revenue (10 EGP per transaction)
            revenueAnalytics["TotalRevenue"] = await _context.Transactions
                .Where(t => t.Status == TransactionStatus.Completed)
                .CountAsync() * SystemWallet.TRANSACTION_FEE; // 10 EGP per transaction

            // Today's Revenue
            var today = DateTime.UtcNow.Date;
            revenueAnalytics["TodayRevenue"] = await _context.Transactions
                .Where(t => t.Status == TransactionStatus.Completed && t.CreatedAt.Date == today)
                .CountAsync() * SystemWallet.TRANSACTION_FEE; // 10 EGP per transaction

            // This Month's Revenue
            var monthStart = new DateTime(today.Year, today.Month, 1);
            revenueAnalytics["MonthlyRevenue"] = await _context.Transactions
                .Where(t => t.Status == TransactionStatus.Completed && t.CreatedAt >= monthStart)
                .CountAsync() * SystemWallet.TRANSACTION_FEE; // 10 EGP per transaction

            // Revenue by Payment Method
            var revenueByMethod = await _context.Transactions
                .Where(t => t.Status == TransactionStatus.Completed)
                .GroupBy(t => t.PaymentMethod)
                .Select(g => new { Method = g.Key, Revenue = g.Count() * SystemWallet.TRANSACTION_FEE })
                .ToDictionaryAsync(x => x.Method, x => x.Revenue);

            // Daily Revenue for Chart
            var dailyRevenue = await _context.Transactions
                .Where(t => t.Status == TransactionStatus.Completed && t.CreatedAt >= monthStart)
                .GroupBy(t => t.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Count() * SystemWallet.TRANSACTION_FEE })
                .ToDictionaryAsync(x => x.Date, x => x.Revenue);

            // Average Revenue per Transaction is always 10 EGP
            revenueAnalytics["AverageRevenue"] = SystemWallet.TRANSACTION_FEE;

            var viewModel = new SystemWalletViewModel
            {
                Wallet = wallet,
                RecentTransactions = transactions,
                TotalIncome = totalIncome,
                TotalPayout = totalPayout,
                TransactionSummary = await transactionsQuery
                    .GroupBy(t => t.Type)
                    .ToDictionaryAsync(g => g.Key, g => g.Sum(t => t.Amount)),
                PaymentMethodStats = await transactionsQuery
                    .GroupBy(t => t.PaymentMethod)
                    .ToDictionaryAsync(g => g.Key, g => g.Count()),
                DailyBalance = await transactionsQuery
                    .GroupBy(t => t.CreatedAt.Date)
                    .ToDictionaryAsync(
                        g => g.Key,
                        g => g.Sum(t => t.Type == TransactionType.CustomerPayment ? t.Amount : -t.Amount)
                    ),
                PendingPayments = await transactionsQuery.CountAsync(t => t.Status == TransactionStatus.Pending),
                CompletedPayments = await transactionsQuery.CountAsync(t => t.Status == TransactionStatus.Completed),
                FailedPayments = await transactionsQuery.CountAsync(t => t.Status == TransactionStatus.Failed),
                PendingPayoutsAmount = await transactionsQuery
                    .Where(t => t.Type == TransactionType.TruckOwnerPayout && t.Status == TransactionStatus.Pending)
                    .SumAsync(t => t.Amount),
                CurrentPage = page,
                TotalPages = totalPages,
                RevenueAnalytics = revenueAnalytics,
                RevenueByPaymentMethod = revenueByMethod,
                DailyRevenue = dailyRevenue,
                TransactionFee = SystemWallet.TRANSACTION_FEE,
                FeeHistory = await _context.TransactionFeeHistory
                    .OrderByDescending(h => h.UpdatedAt)
                    .Take(10) // Show last 10 changes
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayout(int transactionId)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Job)
                .FirstOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null)
                return NotFound();

            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var systemWallet = await _context.SystemWallets.FirstAsync();
                if (systemWallet.Balance < transaction.Amount)
                {
                    TempData["ErrorMessage"] = "Insufficient funds in system wallet";
                    return RedirectToAction(nameof(WalletManagement));
                }

                // Update transaction status
                transaction.Status = TransactionStatus.Completed;
                transaction.CompletedAt = DateTime.UtcNow;

                // Update system wallet
                systemWallet.Balance -= transaction.Amount;
                systemWallet.LastUpdated = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                TempData["SuccessMessage"] = "Payout processed successfully";
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                TempData["ErrorMessage"] = "Failed to process payout";
            }

            return RedirectToAction(nameof(WalletManagement));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTransactionFee(decimal transactionFee)
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

                // Record fee history
                var feeHistory = new TransactionFeeHistory
                {
                    PreviousFee = config.TransactionFee,
                    NewFee = transactionFee,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = User.Identity.Name
                };
                _context.TransactionFeeHistory.Add(feeHistory);

                // Update the fee
                config.TransactionFee = transactionFee;
                config.UpdatedAt = DateTime.UtcNow;

                // Update the static fee in SystemWallet
                SystemWallet.TRANSACTION_FEE = transactionFee;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Transaction fee updated successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating transaction fee: {ex.Message}";
            }

            return RedirectToAction(nameof(WalletManagement));
        }
    }
} 