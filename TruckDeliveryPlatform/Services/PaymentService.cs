using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TruckDeliveryPlatform.Data;
using TruckDeliveryPlatform.Models;

namespace TruckDeliveryPlatform.Services
{
    public class PaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ApplicationDbContext context, ILogger<PaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Transaction> InitiatePayment(Job job, PaymentMethod paymentMethod)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create payment transaction
                var paymentTransaction = new Transaction
                {
                    ReferenceNumber = GenerateReferenceNumber(),
                    Type = TransactionType.CustomerPayment,
                    Amount = job.AcceptedBid.TotalBidAmount,
                    PaymentMethod = paymentMethod,
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

                return paymentTransaction;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error initiating payment: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> VerifyPayment(string referenceNumber)
        {
            var paymentTransaction = await _context.Transactions
                .Include(t => t.Job)
                .FirstOrDefaultAsync(t => t.ReferenceNumber == referenceNumber);

            if (paymentTransaction == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update system wallet
                var systemWallet = await _context.SystemWallets.FirstAsync();
                systemWallet.Balance += paymentTransaction.Amount;
                systemWallet.LastUpdated = DateTime.UtcNow;

                // Update transaction status
                paymentTransaction.Status = TransactionStatus.Completed;
                paymentTransaction.CompletedAt = DateTime.UtcNow;

                // Update job payment status
                paymentTransaction.Job.PaymentStatus = PaymentStatus.Paid;
                paymentTransaction.Job.PaidAmount = paymentTransaction.Amount;
                paymentTransaction.Job.PaidAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error verifying payment: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ProcessTruckOwnerPayout(int jobId)
        {
            var job = await _context.Jobs
                .Include(j => j.AcceptedBid)
                .Include(j => j.Payment)
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null || job.PaymentStatus != PaymentStatus.Paid)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create payout transaction
                var payoutTransaction = new Transaction
                {
                    ReferenceNumber = GenerateReferenceNumber(),
                    Type = TransactionType.TruckOwnerPayout,
                    Amount = job.AcceptedBid.TotalBidAmount,
                    PaymentMethod = job.Payment.PaymentMethod,
                    Status = TransactionStatus.Completed,
                    JobId = job.Id,
                    CustomerId = job.CustomerId,
                    TruckOwnerId = job.AcceptedBid.TruckOwnerId,
                    CreatedAt = DateTime.UtcNow,
                    CompletedAt = DateTime.UtcNow
                };

                _context.Transactions.Add(payoutTransaction);

                // Update system wallet
                var systemWallet = await _context.SystemWallets.FirstAsync();
                systemWallet.Balance -= payoutTransaction.Amount;
                systemWallet.LastUpdated = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error processing payout: {ex.Message}");
                return false;
            }
        }

        private string GenerateReferenceNumber()
        {
            return $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
} 