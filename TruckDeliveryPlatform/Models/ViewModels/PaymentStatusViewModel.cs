using System;
using System.ComponentModel.DataAnnotations;

namespace TruckDeliveryPlatform.Models.ViewModels
{
    public class PaymentStatusViewModel
    {
        public int JobId { get; set; }
        
        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }
        
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }
        
        [Display(Name = "Status")]
        public PaymentStatus Status { get; set; }
        
        // Timestamps for tracking
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? DueDate { get; set; }
        
        // Payment details
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string WalletNumber { get; set; }
        
        // Job details
        public string CustomerName { get; set; }
        public string TruckOwnerName { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        
        // Payment breakdown
        public decimal DeliveryAmount { get; set; }
        public decimal WaitingHourRate { get; set; }
        public decimal EstimatedWaitingHours { get; set; }
        public decimal WaitingAmount => WaitingHourRate * EstimatedWaitingHours;
        public decimal TotalAmount => DeliveryAmount + WaitingAmount;
        
        // Status helpers
        public bool IsPending => Status == PaymentStatus.Pending;
        public bool IsPaid => Status == PaymentStatus.Paid;
        public bool IsOverdue => Status == PaymentStatus.Overdue;
        public bool IsCompleted => Status == PaymentStatus.Completed;
        
        public string StatusMessage
        {
            get
            {
                return Status switch
                {
                    PaymentStatus.Pending => "Payment is pending. Please complete the payment.",
                    PaymentStatus.Paid => "Payment received. Awaiting confirmation.",
                    PaymentStatus.Completed => "Payment completed successfully.",
                    PaymentStatus.Overdue => "Payment is overdue. Please pay as soon as possible.",
                    _ => "Unknown status"
                };
            }
        }

        public string GetPaymentInstructions()
        {
            return PaymentMethod switch
            {
                PaymentMethod.BankTransfer => $"Please transfer {Amount:C} to:\n" +
                                            $"Bank: {BankName}\n" +
                                            $"Account: {AccountNumber}\n" +
                                            $"Name: {AccountName}\n" +
                                            $"Reference: {ReferenceNumber}",
                
                PaymentMethod.VodafoneCash => $"Please send {Amount:C} to:\n" +
                                             $"Wallet: {WalletNumber}\n" +
                                             $"Reference: {ReferenceNumber}",
                
                _ => "Invalid payment method"
            };
        }

        public TimeSpan? TimeUntilDue => DueDate?.Subtract(DateTime.UtcNow);
        public bool IsNearingDue => TimeUntilDue?.TotalHours <= 2;
    }
} 