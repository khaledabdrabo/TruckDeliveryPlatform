using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TruckDeliveryPlatform.Models.ViewModels
{
    public class ProcessPaymentViewModel
    {
        [Required]
        public int? JobId { get; set; }

        [Required]
        public decimal? Amount { get; set; }

        [Required]
        public string? ReferenceNumber { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Payment Details")]
        public string? PaymentDetails { get; set; }

        // Vodafone Cash Details
        [Display(Name = "Wallet Owner Name")]
        public string? WalletOwnerName { get; set; } 

        [Display(Name = "Wallet Number")]
        public string? WalletNumber { get; set; } 

        // Bank Details
        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }

        [Display(Name = "Account Name")]
        public string? AccountName { get; set; }

        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        // For new payment method
        [Display(Name = "Save Payment Method")]
        public bool? SavePaymentMethod { get; set; }

        [Display(Name = "Set as Default")]
        public bool? IsDefault { get; set; }

        // For displaying transaction status
        public TransactionStatus? TransactionStatus { get; set; }
    }
} 