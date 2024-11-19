using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TruckDeliveryPlatform.Data;
using TruckDeliveryPlatform.Models;
using TruckDeliveryPlatform.Models.ViewModels;

namespace TruckDeliveryPlatform.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ProcessJobPayment(int jobId)
        {
            var job = await _context.Jobs
                .Include(j => j.AcceptedBid)
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null || job.CustomerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            if (job.PaymentStatus != PaymentStatus.Pending)
                return BadRequest("Payment already processed");

            if (job.AcceptedBid == null)
                return BadRequest("No accepted bid found for this job");

            var paymentMethods = await _context.PaymentDetails
                .Where(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .ToListAsync();

            var referenceNumber = GenerateReferenceNumber();

            var viewModel = new ProcessPaymentViewModel
            {
                JobId = job.Id,
                Amount = job.AcceptedBid.BidAmount,//+ (job.EstimatedWaitingHours * job.AcceptedBid.WaitingHourPrice),
                //PaymentMethods = paymentMethods,
                //AvailablePaymentMethods = Enum.GetValues<PaymentMethod>().ToList(),
                ReferenceNumber = referenceNumber
            };

            // Store reference number in TempData to verify it hasn't been tampered with
            TempData["ReferenceNumber"] = referenceNumber;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessJobPayment(ProcessPaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Verify reference number matches what we generated
            if (TempData["ReferenceNumber"]?.ToString() != model.ReferenceNumber)
            {
                return BadRequest("Invalid reference number");
            }

            var job = await _context.Jobs
                .Include(j => j.AcceptedBid)
                .FirstOrDefaultAsync(j => j.Id == model.JobId);

            if (job == null || job.CustomerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            if (job.AcceptedBid == null)
                return BadRequest("No accepted bid found for this job");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var totalAmount = job.AcceptedBid.BidAmount + (job.EstimatedWaitingHours * job.AcceptedBid.WaitingHourPrice);

                // Create payment transaction
                var paymentTransaction = new Transaction
                {
                    ReferenceNumber = model.ReferenceNumber,
                    Type = TransactionType.CustomerPayment,
                    Amount = totalAmount,
                    PaymentMethod = model.PaymentMethod,
                    PaymentDetails = "",//model.PaymentDetails,
                    Status = TransactionStatus.Pending,
                    JobId = job.Id,
                    CustomerId = job.CustomerId,
                    TruckOwnerId = job.AcceptedBid.TruckOwnerId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Transactions.Add(paymentTransaction);

                // Update job payment status
                job.PaymentStatus = PaymentStatus.Pending;
                job.PaymentDueDate = DateTime.UtcNow.AddDays(1);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Redirect to payment gateway or show payment instructions
                return RedirectToAction("PaymentInstructions", new { id = paymentTransaction.Id });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> PaymentInstructions(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Job)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return NotFound();

            if (transaction.CustomerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            // Return the transaction directly since our view expects Transaction model
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPayment(string referenceNumber)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Job)
                .FirstOrDefaultAsync(t => t.ReferenceNumber == referenceNumber);

            if (transaction == null)
                return NotFound();

            if (transaction.CustomerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update transaction status
                transaction.Status = TransactionStatus.Completed;
                transaction.CompletedAt = DateTime.UtcNow;

                // Update job payment status
                transaction.Job.PaymentStatus = PaymentStatus.Paid;
                transaction.Job.PaidAt = DateTime.UtcNow;
                transaction.Job.PaidAmount = transaction.Amount;
                transaction.Job.Status = JobStatus.InProgress; // Update job status to InProgress

                // Update system wallet
                var systemWallet = await _context.SystemWallets.FirstOrDefaultAsync();
                if (systemWallet == null)
                {
                    systemWallet = new SystemWallet { Balance = 0, LastUpdated = DateTime.UtcNow };
                    _context.SystemWallets.Add(systemWallet);
                }
                systemWallet.Balance += transaction.Amount;
                systemWallet.LastUpdated = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                TempData["SuccessMessage"] = "Payment verified successfully. Your job is now in progress.";
                return RedirectToAction("Details", "Jobs", new { id = transaction.JobId });
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                TempData["ErrorMessage"] = "Failed to verify payment. Please try again.";
                return RedirectToAction("PaymentInstructions", new { id = transaction.Id });
            }
        }

        private string GenerateReferenceNumber()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999).ToString();
            return $"PAY-{timestamp}-{random}";
        }
    }
}